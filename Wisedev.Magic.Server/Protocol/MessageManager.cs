using MongoDB.Driver;
using System.Threading.Tasks;
using Wisedev.Magic.Logic;
using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Entries;
using Wisedev.Magic.Logic.Message.Alliance;
using Wisedev.Magic.Logic.Message.Auth;
using Wisedev.Magic.Logic.Message.Home;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Server.Lgoic.Mode;
using Wisedev.Magic.Server.Network.Connection;
using Wisedev.Magic.Server.Resources;
using Wisedev.Magic.Server.Util;
using Wisedev.Magic.Titam.Logic;
using Wisedev.Magic.Titam.Message;
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
            case 14102:
                await this.OnEndClientTurnReceived((EndClientTurnMessage)message);
                break;
            case CreateAllianceMessage.MESSAGE_TYPE:
                await OnCreateAllianceMessageReceived((CreateAllianceMessage)message);
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
            gameMode.OnClientTurnReceived(message.GetSubTick(), message.GetChecksum(), message.GetCommands());
    }

    private async Task OnCreateAllianceMessageReceived(CreateAllianceMessage createAllianceMessage)
    {
        Debugger.Print($"Tryna create alliance: name={createAllianceMessage.GetAllianceName()}, badge_id={createAllianceMessage.GetAllianceBadgeData().GetGlobalID()}, type={createAllianceMessage.GetAllianceType()}, required_score={createAllianceMessage.GetRequiredScore()} desc={createAllianceMessage.GetAllianceDescription()}");

        Alliance? alliance = null;

        alliance = await this._allianceRepository.CreateAsync(createAllianceMessage.GetAllianceName(), createAllianceMessage.GetAllianceDescription());

        AllianceHeaderEntry allianceHeader = new AllianceHeaderEntry();
        allianceHeader.SetAllianceId(1);
        allianceHeader.SetAllianceName(createAllianceMessage.GetAllianceName());
        allianceHeader.SetAllianceBadgeData(createAllianceMessage.GetAllianceBadgeData());
        allianceHeader.SetAllianceType(createAllianceMessage.GetAllianceType());
        allianceHeader.SetNumberOfMembers(1);
        allianceHeader.SetScore(1);
        allianceHeader.SetRequiredScore(createAllianceMessage.GetRequiredScore());

        List<AllianceMemberEntry> allianceMembers = new List<AllianceMemberEntry>(1)
        {
            new()
        };

        allianceMembers[0].SetAvatarId(1);
        allianceMembers[0].SetFacebookId(string.Empty);
        allianceMembers[0].SetName("test_0");
        allianceMembers[0].SetRole(1);
        allianceMembers[0].SetExpLevel(1);
        allianceMembers[0].SetLeagueType((LogicLeagueData)LogicDataTables.GetTable(LogicDataType.LEAGUE).GetItemAt(1));
        allianceMembers[0].SetScore(1);
        allianceMembers[0].SetDonations(1);
        allianceMembers[0].SetDonationsReceived(1);
        allianceMembers[0].SetOrder(2);
        allianceMembers[0].SetPreviousOrder(1);
        allianceMembers[0].SetNewMember(false);
        allianceMembers[0].SetHomeId(1);

        AllianceFullEntry allianceFullEntry = new AllianceFullEntry();
        allianceFullEntry.SetAllianceHeaderEntry(allianceHeader);
        allianceFullEntry.SetAllianceMembers(allianceMembers);
        allianceFullEntry.SetAllianceDescription(createAllianceMessage.GetAllianceDescription());

        AllianceDataMessage allianceDataMessage = new AllianceDataMessage();
        allianceDataMessage.SetAllianceFullEntry(allianceFullEntry);

        await _connection.SendMessage(allianceDataMessage);

    }
}
