using MongoDB.Driver;
using System.Threading.Tasks;
using Wisedev.Magic.Logic;
using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Command.Debug;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Entries;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Message.Alliance;
using Wisedev.Magic.Logic.Message.Auth;
using Wisedev.Magic.Logic.Message.Home;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Server.Lgoic.Mode;
using Wisedev.Magic.Server.Network.Connection;
using Wisedev.Magic.Server.Resources;
using Wisedev.Magic.Server.Util;
using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Message;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Protocol;

class MessageManager
{
    private ClientConnection _connection;
    private readonly IAccountRepository _accountRepository;
    private readonly IAllianceRepository _allianceRepository;

    public MessageManager(ClientConnection connection, IAccountRepository accountRepository, IAllianceRepository allianceRepository)
    {
        this._connection = connection;
        this._accountRepository = accountRepository;
        this._allianceRepository = allianceRepository;
    }

    public async Task ReceiveMessage(PiranhaMessage message)
    {
        int messageType = message.GetMessageType();

        if (messageType != 14102)
            Debugger.Print($"MessageManager.ReceiveMessage: type={messageType}, name=" + message.GetType().Name);

        switch (messageType)
        {
            case 10101:
                await this.OnLoginMessageReceived((LoginMessage)message);
                break;
            case GoHomeMessage.MESSAGE_TYPE:
                await this.OnGoHomeMessageReceived((GoHomeMessage)message);
                break;
            case 14102:
                await this.OnEndClientTurnReceived((EndClientTurnMessage)message);
                break;
            case CreateAllianceMessage.MESSAGE_TYPE:
                await OnCreateAllianceMessageReceived((CreateAllianceMessage)message);
                break;
            case AttackNpcMessage.MESSAGE_TYPE:
                await this.OnAttackMessageReceived((AttackNpcMessage)message);
                break;
            case 14325:
                await this.OnAskForAvatarProfileMessage((AskForAvatarProfileMessage)message);
                break;
        }
    }

    private async Task OnLoginMessageReceived(LoginMessage loginMessage)
    {
        LogicLong accountId = loginMessage.RemoveAccountId();
        string? passToken = loginMessage.RemovePassToken();

        Debugger.Print($"Tryna login id={accountId}, passToken={passToken}, client version={loginMessage.GetMajorVersion()}.{loginMessage.GetBuild()}.{loginMessage.GetMinorVersion() + 1}, server version={LogicVersion.MAJOR_VERSION}.{LogicVersion.BUILD}.{LogicVersion.CONTENT_VERSION}, device language={loginMessage.GetPreferredDeviceLanguage()}");
        Debugger.Print($"client sha={loginMessage.GetResourceSHA()}");

        if (!await this.CheckClientVersion(loginMessage.GetMajorVersion(), loginMessage.GetBuild(), loginMessage.GetResourceSHA()))
        {
            Debugger.Print("Client version is invalid, rejecting login.");
            return;
        }

        Account? account = null;

        if (accountId.High != 0 || accountId.Low != 0)
        {
            account = await this._accountRepository.GetByIdAsync(accountId);

            if (account == null || account.PassToken != passToken)
            {
                Debugger.Print("Invalid account or passToken, rejecting login.");
                return;
            }

            await _accountRepository.UpdateAccountAsync(accountId, Builders<Account>.Update
                                        .Set(a => a.LastLoginAt, DateTime.UtcNow));

            var timeSinceLastLogin = DateTime.UtcNow - account.LastLoginAt;
            var totalPlayTime = account.PlayTimeSeconds + (int)timeSinceLastLogin.TotalSeconds;

            await _accountRepository.UpdateAccountAsync(accountId, Builders<Account>.Update
                                        .Set(a => a.PlayTimeSeconds, totalPlayTime));

            var daysSinceStarted = (DateTime.UtcNow - account.CreatedAt).Days;
            await _accountRepository.UpdateAccountAsync(accountId, Builders<Account>.Update
                                        .Set(a => a.DaysSinceStartedPlaying, daysSinceStarted));
        }
        else
        {
            Debugger.Print("Creating new account...");
            account = await this._accountRepository.CreateAsync();
        }

        this._connection.SetCurrentAccountId(account.Id);

        LoginOkMessage loginOkMessage = new LoginOkMessage();
        loginOkMessage.SetAccountId(account.Id);
        loginOkMessage.SetHomeId(account.Id);
        loginOkMessage.SetPassToken(account.PassToken);
        loginOkMessage.SetFacebookId(null);
        loginOkMessage.SetGamecenterId(null);
        loginOkMessage.SetMajorVersion(LogicVersion.MAJOR_VERSION);
        loginOkMessage.SetBuild(LogicVersion.BUILD);
        loginOkMessage.SetContentVersion(LogicVersion.CONTENT_VERSION);
        loginOkMessage.SetEnvironment(EnvironmentUtil.GetEnvironmentAbbreviation(Config.Environment));
        loginOkMessage.SetSessionCount(account.SessionCount);
        loginOkMessage.SetPlayTimeSeconds(account.PlayTimeSeconds);
        loginOkMessage.SetDaysSinceStartedPlaying(account.DaysSinceStartedPlaying);
        loginOkMessage.SetFacebookAppId(null);
        loginOkMessage.SetServerTime(DateTime.UtcNow.ToString());
        loginOkMessage.SetAccountCreatedDate(account.CreatedAt.ToString());
        loginOkMessage.SetStartupCooldownSeconds(account.StartupCooldownSeconds);
        loginOkMessage.SetGoogleServiceId(null);

        await _connection.SendMessage(loginOkMessage);

        account.ClientAvatar.SetUnitCount((LogicCharacterData)LogicDataTables.GetTable(LogicDataType.CHARACTER).GetItemAt(0), 5);

        this._connection.SetAccountDocument(account);
        this._connection.SetGameMode(await GameMode.LoadHomeState(this._connection, account.Home, account.ClientAvatar));
    }

    private async ValueTask<bool> CheckClientVersion(int majorVersion, int build, string resourceSha)
    {
        if (majorVersion != LogicVersion.MAJOR_VERSION || build != LogicVersion.BUILD)
        {
            LoginFailedMessage loginFailedMessage = new LoginFailedMessage();
            loginFailedMessage.SetErrorCode(LoginFailedMessage.ErrorCode.CLIENT_VERSION);
            loginFailedMessage.SetUpdateURL("https://api.bladewise.xyz/supercell");

            await this._connection.SendMessage(loginFailedMessage);
            return false;
        }

        if (resourceSha != ResourceManager.FINGERPRINT_SHA)
        {
            LoginFailedMessage loginFailedMessage = new LoginFailedMessage();
            loginFailedMessage.SetErrorCode(LoginFailedMessage.ErrorCode.DATA_VERSION);
            loginFailedMessage.SetContentURL("https://api.bladewise.xyz/supercell");
            loginFailedMessage.SetResourceFingerprintData(ResourceManager.FINGERPRINT_JSON!);

            await this._connection.SendMessage(loginFailedMessage);
            return false;
        }

        Debugger.Print("Client version mathces the server version!");
        return true;
    }

    private async Task OnAskForAvatarProfileMessage(AskForAvatarProfileMessage askProfileMessage)
    {
        Account? account = await this._accountRepository.GetByIdAsync(askProfileMessage.GetAvatarId());
        if (account == null)
        {
            Debugger.Print($"MessageManager.OnAskForAvatarProfileMessage: Account ({askProfileMessage.GetAvatarId()}) not found!");
            return;
        }

        AvatarProfileMessage avatarProfileMessage = new AvatarProfileMessage();

        AvatarProfileFullEntry avatarProfileEntry = new AvatarProfileFullEntry();
        avatarProfileEntry.SetLogicClientAvatar(account.ClientAvatar);
        avatarProfileEntry.SetDonations(0); // TODO: rewrite hardcode
        avatarProfileEntry.SetDonationsReceived(0); // TODO: rewrite hardcode

        avatarProfileMessage.SetAvatarProfileFullEntry(avatarProfileEntry);

        await this._connection.SendMessage(avatarProfileMessage);
    }

    private async Task OnEndClientTurnReceived(EndClientTurnMessage message)
    {
        GameMode gameMode = this._connection.GetGameMode();

        if (gameMode != null)
        {
            gameMode.OnClientTurnReceived(message.GetSubTick(), message.GetChecksum(), message.GetCommands());

            await this._accountRepository.UpdateAccountAsync(this._connection.GetAccountDocument());
        }
    }

    private async Task OnGoHomeMessageReceived(GoHomeMessage message)
    {
        this._connection.SetGameMode(await GameMode.LoadHomeState(this._connection, this._connection.GetAccountDocument().Home, 
            this._connection.GetAccountDocument().ClientAvatar));
    }

    private async Task OnAttackMessageReceived(AttackNpcMessage attackNpcMessage)
    {
        Debugger.Print($"Received AttackNpcMessage: targetNpcId={attackNpcMessage.GetNpcData().GetGlobalID()}");

        LogicLevel level = this._connection.GetGameMode().GetLogicGameMode().GetLevel();
        LogicNpcAvatar npcAvatar = new LogicNpcAvatar();
        npcAvatar.SetNpcData(attackNpcMessage.GetNpcData());


        GameMode? gameMode = await GameMode.LoadNpcAttackState(_connection,
            level.GetHome(), level.GetPlayerAvatar(), npcAvatar);

        if (gameMode != null)
        {
            _connection.SetGameMode(gameMode);

            Debugger.Print("MessageManager.OnAttackMessageReceived: NPC attack state loaded successfully.");
        }
        else
        {
            Debugger.Print("Failed to load NPC attack state.");
        }
    }

    private async Task OnCreateAllianceMessageReceived(CreateAllianceMessage createAllianceMessage)
    {
        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
        message.SetServerCommand(new LogicDebugCommand(3));
        await _connection.SendMessage(message);

        Debugger.Print($"Tryna create alliance: name={createAllianceMessage.GetAllianceName()}, badge_id={createAllianceMessage.GetAllianceBadgeData().GetGlobalID()}, type={createAllianceMessage.GetAllianceType()}, required_score={createAllianceMessage.GetRequiredScore()} desc={createAllianceMessage.GetAllianceDescription()}");


    }
}
