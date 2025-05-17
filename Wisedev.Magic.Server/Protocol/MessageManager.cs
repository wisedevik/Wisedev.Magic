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
using Wisedev.Magic.Logic.Command.Home;
using Wisedev.Magic.Logic.Command.Alliance;
using Wisedev.Magic.Server.Network;
using Wisedev.Magic.Logic.Notifications;

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

        if (messageType != EndClientTurnMessage.MESSAGE_TYPE)
            Debugger.Print($"MessageManager.ReceiveMessage: type={messageType}, name=" + message.GetType().Name);

        switch (messageType)
        {
            case LoginMessage.MESSAGE_TYPE:
                await this.OnLoginMessageReceived((LoginMessage)message);
                break;
            case ReportUserMessage.MESSAGE_TYPE:
                await this.OnReportUserMessage((ReportUserMessage)message);
                break;
            case GoHomeMessage.MESSAGE_TYPE:
                await this.OnGoHomeMessageReceived((GoHomeMessage)message);
                break;
            case EndClientTurnMessage.MESSAGE_TYPE:
                await this.OnEndClientTurnReceived((EndClientTurnMessage)message);
                break;
            case CreateAllianceMessage.MESSAGE_TYPE:
                await this.OnCreateAllianceMessageReceived((CreateAllianceMessage)message);
                break;
            case AttackNpcMessage.MESSAGE_TYPE:
                await this.OnAttackMessageReceived((AttackNpcMessage)message);
                break;
            case AskForAvatarProfileMessage.MESSAGE_TYPE:
                await this.OnAskForAvatarProfileMessage((AskForAvatarProfileMessage)message);
                break;
            case SendGlobalChatLineMessage.MESSAGE_TYPE:
                await this.OnSendGlobalChatLineMessage((SendGlobalChatLineMessage)message);
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


            var timeSinceLastLogin = DateTime.UtcNow - account.LastLoginAt;
            var totalPlayTime = account.PlayTimeSeconds + (int)timeSinceLastLogin.TotalSeconds;
            var daysSinceStarted = (DateTime.UtcNow - account.CreatedAt).Days;

            _accountRepository.UpdateAccountAsync(account.Id, Builders<Account>.Update
                .Set(a => a.LastLoginAt, DateTime.UtcNow)
                .Set(a => a.PlayTimeSeconds, totalPlayTime)
                .Set(a => a.DaysSinceStartedPlaying, daysSinceStarted));
        }
        else
        {
            Debugger.Print("Creating new account...");
            account = await this._accountRepository.CreateAsync();
        }

        this._connection.SetCurrentAccountId(account.Id);

        Console.WriteLine($"BanEndTime: {account.BanEndTime}");
        if (account.BanEndTime != null && account.BanEndTime > DateTime.UtcNow)
        {
            Console.WriteLine($"SUKA BAN! {account.Id}");
            var remainingBanTime = account.BanEndTime.Value - DateTime.UtcNow;
            await _connection.SendMessage(new ChatAccountBanStatusMessage()
            {
                BanTime = (int)remainingBanTime.TotalMilliseconds
            });
            Console.WriteLine($"[BAN] {account.Id} is banned until {account.BanEndTime}");
        }

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
            loginFailedMessage.SetUpdateURL($"{Config.ApiUrl}");

            await this._connection.SendMessage(loginFailedMessage);
            return false;
        }

        if (resourceSha != ResourceManager.FINGERPRINT_SHA)
        {
            LoginFailedMessage loginFailedMessage = new LoginFailedMessage();
            loginFailedMessage.SetErrorCode(LoginFailedMessage.ErrorCode.DATA_VERSION);
            loginFailedMessage.SetContentURL($"{Config.ApiUrl}");
            loginFailedMessage.SetResourceFingerprintData(ResourceManager.FINGERPRINT_JSON!);

            await this._connection.SendMessage(loginFailedMessage);
            return false;
        }

        Debugger.Print("Client version mathces the server version!");
        return true;
    }

    public async Task OnReportUserMessage(ReportUserMessage message)
    {
        Console.WriteLine($"[REPORT] Reported to {message.ReportedAvatarId} ({message.Count})");
        var reportedId = message.ReportedAvatarId;
        var account = await _accountRepository.GetByIdAsync(reportedId);
        if (account == null)
            return;

        Console.WriteLine($"[LOADED] {account.Id} {account.CreatedAt} {account.PlayTimeSeconds}");

        if (account.ReportCount >= 1)
        {
            DateTime banEndTime = DateTime.UtcNow.AddMinutes(5);
            account.BanEndTime = banEndTime;

            if (_connection.GetClientConnectionManager().GetActiveConnections().TryGetValue(reportedId, out var connection))
            {
                await connection.SendMessage(new ChatAccountBanStatusMessage()
                {
                    BanTime = (int)(banEndTime - DateTime.UtcNow).TotalMilliseconds
                });
            }

            await _accountRepository.UpdateAccountAsync(reportedId, Builders<Account>.Update
                .Set(a => a.BanEndTime, banEndTime)
                .Set(a => a.ReportCount, account.ReportCount + 1));

            Console.WriteLine($"[BAN] {reportedId} banned until {account.BanEndTime}");
        }
    }

    private async Task OnSendGlobalChatLineMessage(SendGlobalChatLineMessage message)
    {
        string? playerMessage = message.GetMessage();
        if (playerMessage.Length > 0)
        {
            if (playerMessage.Length > 128)
                playerMessage = playerMessage.Substring(0, 128);
            if (playerMessage.StartsWith("/"))
            {
                LogicClientAvatar logicClientAvatar = _connection.GetAccountDocument().ClientAvatar;
                GlobalChatLineMessage globalChatLineMessage = new GlobalChatLineMessage();

                globalChatLineMessage.SetAvatarName(logicClientAvatar.GetName());
                globalChatLineMessage.SetExpLvl(logicClientAvatar.GetExpLevel());
                globalChatLineMessage.SetLeagueType(logicClientAvatar.LeagueType);
                globalChatLineMessage.SetAvatarId(logicClientAvatar.Id);
                globalChatLineMessage.SetHomeId(logicClientAvatar.CurrentHomeId);

                if (logicClientAvatar.IsInAlliance())
                {
                    globalChatLineMessage.SetAllianceId(logicClientAvatar.AllianceId);
                }

                string commandLine = playerMessage.Substring(1);
                string[] commandParts = commandLine.Split(' ');

                string command = commandParts[0].ToLower();

                switch (command)
                {
                    case "help":
                        string helpMessage = "Available commands: /help, /info";
                        globalChatLineMessage.SetMessage(helpMessage);
                        break;
                    case "info":
                        TimeSpan uptime = DateTime.UtcNow - ServerStats.StartTime;
                        string formattedUptime = $"{(int)uptime.TotalHours:D2}:{uptime.Minutes:D2}:{uptime.Seconds:D2}";
                        string currentTime = DateTime.UtcNow.ToString("HH:mm") + " (UTC)";

                        var proc = System.Diagnostics.Process.GetCurrentProcess();
                        long workingSetBytes = proc.WorkingSet64;
                        int usedMemoryMB = (int)(workingSetBytes / 1024 / 1024);

                        long managed = GC.GetTotalMemory(false) / 1024 / 1024;
                        long total = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;

                        string info = $"Managed: {managed} MB | Total: {total} MB";

                        int gcGen0 = GC.CollectionCount(0);
                        int gcGen1 = GC.CollectionCount(1);
                        int gcGen2 = GC.CollectionCount(2);

                        string infoMessage =
                            $"[SERVER INFO]\n" +
                            $"Version: {LogicVersion.MAJOR_VERSION}.{LogicVersion.BUILD}.{LogicVersion.CONTENT_VERSION}\n" +
                            $"Uptime: {formattedUptime}\n" +
                            $"Players Online: {_connection.GetClientConnectionManager().GetOnlineCount()}\n" +
                            $"Time: {currentTime}\n\n" +
                            $"[Performance]\n" +
                            $"Used Total Memory: {usedMemoryMB} MB\n" +
                            $"GC Gen0: {gcGen0} / Gen1: {gcGen1} / Gen2: {gcGen2}";

                        globalChatLineMessage.SetMessage(infoMessage);
                        break;
                    case "add_d":
                        if (commandParts.Length > 1 && int.TryParse(commandParts[1], out int amount))
                        {
                            LogicDiamondsAddedCommand logicDiamondsAddedCommand = new LogicDiamondsAddedCommand();
                            logicDiamondsAddedCommand.Amount = amount;

                            AvailableServerCommandMessage availableServerCommandMessage = new AvailableServerCommandMessage();
                            availableServerCommandMessage.SetServerCommand(logicDiamondsAddedCommand);

                            await _connection.SendMessage(availableServerCommandMessage);

                            globalChatLineMessage.SetMessage($"Added {amount} diamonds.");
                        }
                        else
                        {
                            globalChatLineMessage.SetMessage("Usage: /add_d <amount>");
                        }
                        break;
                    default:
                        globalChatLineMessage.SetMessage("Unknown command. Type /help for a list of commands.");
                        break;
                }

                await _connection.SendMessage(globalChatLineMessage);
            }
            else
            {
                await _connection.ChatInstance.PublishMessage(_connection.GetAccountDocument().ClientAvatar, playerMessage);
            }
        }
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

    private async Task SaveAccountData(bool fullSave = false)
    {
        var account = this._connection.GetAccountDocument();
        if (fullSave)
        {
            await _accountRepository.UpdateAccountAsync(account);
        }
        else
        {
            await _accountRepository.UpdateAccountAsync(account.Id, Builders<Account>.Update
                .Set(a => a.ClientAvatar, account.ClientAvatar)
                .Set(a => a.Home, account.Home));
        }
    }

    private async Task OnEndClientTurnReceived(EndClientTurnMessage message)
    {
        GameMode gameMode = this._connection.GetGameMode();

        if (gameMode != null)
        {
            gameMode.OnClientTurnReceived(message.GetSubTick(), message.GetChecksum(), message.GetCommands());

            Account account = _connection.GetAccountDocument();
            await _accountRepository.UpdateAccountAsync(account.Id, Builders<Account>.Update
                .Set(a => a.ClientAvatar, account.ClientAvatar)
                .Set(a => a.Home, account.Home)
                .Set(a => a.LastSaveTime, DateTime.UtcNow));
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
        Debugger.Print($"Tryna create alliance: name={createAllianceMessage.GetAllianceName()}, badge_id={createAllianceMessage.GetAllianceBadgeData().GetGlobalID()}, type={createAllianceMessage.GetAllianceType()}, required_score={createAllianceMessage.GetRequiredScore()} desc={createAllianceMessage.GetAllianceDescription()}");

        AllianceDataMessage allianceDataMessage = new AllianceDataMessage();
        AllianceFullEntry allianceFullEntry = new AllianceFullEntry();
        AllianceHeaderEntry allianceHeaderEntry = new AllianceHeaderEntry();

        allianceHeaderEntry.SetAllianceId(1);
        allianceHeaderEntry.SetAllianceName(createAllianceMessage.GetAllianceName());
        allianceHeaderEntry.SetAllianceBadgeData(createAllianceMessage.GetAllianceBadgeData());
        allianceHeaderEntry.SetAllianceType(createAllianceMessage.GetAllianceType());
        allianceHeaderEntry.SetNumberOfMembers(1);
        allianceHeaderEntry.SetScore(999);
        allianceHeaderEntry.SetRequiredScore(createAllianceMessage.GetRequiredScore());

        allianceFullEntry.SetAllianceHeaderEntry(allianceHeaderEntry);

        AllianceMemberEntry allianceMemberEntry = new AllianceMemberEntry();
        allianceMemberEntry.SetAvatarId(this._connection.GetCurrentAccountId());
        allianceMemberEntry.SetFacebookId(null);
        allianceMemberEntry.SetName(this._connection.GetAccountDocument().ClientAvatar.GetName());
        allianceMemberEntry.SetRole(1);
        allianceMemberEntry.SetExpLevel(this._connection.GetAccountDocument().ClientAvatar.GetExpLevel());
        allianceMemberEntry.SetLeagueType((LogicLeagueData)LogicDataTables.GetDataById(29000000));
        allianceMemberEntry.SetScore(999);
        allianceMemberEntry.SetDonations(0);
        allianceMemberEntry.SetDonationsReceived(0);
        allianceMemberEntry.SetOrder(1);
        allianceMemberEntry.SetPreviousOrder(1);
        allianceMemberEntry.SetNewMember(true);
        allianceMemberEntry.SetHomeId(this._connection.GetCurrentAccountId());

        allianceFullEntry.SetAllianceMembers(new List<AllianceMemberEntry>()
        {
            allianceMemberEntry
        });

        allianceFullEntry.SetAllianceDescription(createAllianceMessage.GetAllianceDescription());

        allianceDataMessage.SetAllianceFullEntry(allianceFullEntry);

        await this._connection.SendMessage(allianceDataMessage);
        LogicJoinAllianceCommand logicJoinAllianceCommand = new LogicJoinAllianceCommand()
        {
            AllianceId = 1,
            AllianceName = createAllianceMessage.GetAllianceName(),
            AllianceBadgeData = createAllianceMessage.GetAllianceBadgeData(),
            IsAllianceCreator = false,
        };

        AvailableServerCommandMessage availableServerCommandMessage = new AvailableServerCommandMessage();
        availableServerCommandMessage.SetServerCommand(logicJoinAllianceCommand);
        await this._connection.SendMessage(availableServerCommandMessage);
    }
}
