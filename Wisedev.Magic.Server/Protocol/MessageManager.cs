using System.Threading.Tasks;
using Wisedev.Magic.Logic;
using Wisedev.Magic.Logic.Message.Auth;
using Wisedev.Magic.Logic.Message.Home;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Server.Network.Connection;
using Wisedev.Magic.Server.Resources;
using Wisedev.Magic.Titam.Logic;
using Wisedev.Magic.Titam.Message;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Protocol;

class MessageManager
{
    private ClientConnection _connection;
    private readonly IAccountRepository _accountRepository;

    public MessageManager(ClientConnection connection, IAccountRepository accountRepository)
    {
        this._connection = connection;
        this._accountRepository = accountRepository;
    }

    public async Task ReceiveMessage(PiranhaMessage message)
    {
        int messageType = message.GetMessageType();
        Debugger.Print($"MessageManager.ReceiveMessage: type={messageType}, name=" + message.GetType().Name);

        switch (messageType)
        {
            case 10101:
                await this.OnLoginMessageReceived((LoginMessage)message);
                break;
        }
    }

    private async Task OnLoginMessageReceived(LoginMessage loginMessage)
    {
        LogicLong accountId = loginMessage.RemoveAccountId();
        string? passToken = loginMessage.RemovePassToken();

        Debugger.Print($"Tryna login id={accountId}, passToken={passToken}, client version={loginMessage.GetMajorVersion()}.{loginMessage.GetBuild()}.{loginMessage.GetMinorVersion() + 1}, server version={LogicVersion.MAJOR_VERSION}.{LogicVersion.BUILD}.{LogicVersion.CONTENT_VERSION}, device language={loginMessage.GetPreferredDeviceLanguage()}");
        Debugger.Print($"client sha={loginMessage.GetResourceSHA()}");

        if (await this.CheckClientVersion(loginMessage.GetMajorVersion(), loginMessage.GetBuild(), loginMessage.GetResourceSHA()))
        {
            Debugger.Print("Client version is invalid, rejecting login.");
            return;
        }

        Account? account = null;

        if (accountId.High != 0 || accountId.Low != 0)
        {
            account = await _accountRepository.GetByIdAsync(accountId);

            if (account == null || account.PassToken != passToken)
            {
                Debugger.Print("Invalid account or passToken, rejecting login.");
                return;
            }
        }
        else
        {
            Debugger.Print("Creating new account...");
            account = await _accountRepository.CreateAsync();
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
        loginOkMessage.SetEnvironment("int");
        loginOkMessage.SetSessionCount(1);
        loginOkMessage.SetPlayTimeSeconds(0);
        loginOkMessage.SetDaysSinceStartedPlaying(0);
        loginOkMessage.SetFacebookAppId(null);
        loginOkMessage.SetServerTime(DateTime.UtcNow.ToString());
        loginOkMessage.SetAccountCreatedDate(account.CreatedAt.ToString());
        loginOkMessage.SetStartupCooldownSeconds(0);
        loginOkMessage.SetGoogleServiceId(null);

        await _connection.SendMessage(loginOkMessage);

        OwnHomeDataMessage ownHomeDataMessage = new OwnHomeDataMessage();
        ownHomeDataMessage.SetSecondsSinceLastSave(-1);
        ownHomeDataMessage.SetLogicClientHome(account.Home);
        ownHomeDataMessage.SetLogicClientAvatar(account.ClientAvatar);

        await this._connection.SendMessage(ownHomeDataMessage);
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
            loginFailedMessage.SetResourceFingerprintData(ResourceManager.FINGERPRINT_JSON);

            await this._connection.SendMessage(loginFailedMessage);
            return false;
        }

        Debugger.Print("Client version mathces the server version!");
        return true;
    }
}
