using Wisedev.Magic.Titam.Logic;
using Wisedev.Magic.Titam.Message;

namespace Wisedev.Magic.Logic.Message.Auth;

[PiranhaMessage(10101)]
public class LoginMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 10101;

    private LogicLong _accountId;
    private string? _passToken;
    private int _majorVersion;
    private int _minorVersion;
    private int _build;
    private string? _resourceSHA;
    private string? _preferredDeviceLanguage;

    public LoginMessage() : base(0)
    {
        this._accountId = new LogicLong();
        this._passToken = null;
        this._majorVersion = 0;
        this._minorVersion = 0;
        this._build = 0;
        this._resourceSHA = null;
        this._preferredDeviceLanguage = null;
    }

    public override void Decode()
    {
        base.Decode();
        this._accountId = this._stream.ReadLong();
        this._passToken = this._stream.ReadString();
        this._majorVersion = this._stream.ReadInt();
        this._minorVersion = this._stream.ReadInt(); // hardcode by sc
        this._build = this._stream.ReadInt();
        this._resourceSHA = this._stream.ReadString();
        this._stream.ReadString(); // TODO!;
        this._stream.ReadString(); // TODO!;
        this._stream.ReadString(); // TODO!;
        this._stream.ReadString(); // TODO!;
        this._stream.ReadInt();
        this._preferredDeviceLanguage = this._stream.ReadString();
    }

    public LogicLong RemoveAccountId()
    {
        LogicLong accId = this._accountId;
        this._accountId = null;
        return accId;
    }

    public string? RemovePassToken()
    {
        string? passT = this._passToken;
        this._passToken = null;
        return passT;
    }

    public LogicLong GetAccountId()
    {
        return this._accountId;
    }

    public string? GetPassToken()
    {
        return this._passToken;
    }

    public int GetMajorVersion()
    {
        return this._majorVersion;
    }

    public int GetMinorVersion()
    {
        return this._minorVersion;
    }

    public int GetBuild()
    {
        return this._build;
    }

    public string GetResourceSHA()
    {
        return this._resourceSHA!;
    }

    public string GetPreferredDeviceLanguage()
    {
        return this._preferredDeviceLanguage!;
    }

    public string RemovePreferredDeviceLanguage()
    {
        string? preferredDeviceLanguage = this._preferredDeviceLanguage;
        this._preferredDeviceLanguage = null;
        return preferredDeviceLanguage!;
    }

    public override short GetMessageType()
    {
        return LoginMessage.MESSAGE_TYPE;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}
