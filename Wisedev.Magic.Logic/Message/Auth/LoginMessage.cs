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
    public string? ResourceSHA;

    public LoginMessage() : base(0)
    {
    }

    public override void Decode()
    {
        base.Decode();
        this._accountId = this._stream.ReadLong();
        this._passToken = this._stream.ReadString();
        this._majorVersion = this._stream.ReadInt();
        this._minorVersion = this._stream.ReadInt(); // hardcode by sc
        this._build = this._stream.ReadInt();
        this.ResourceSHA = this._stream.ReadString();
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

    public override short GetMessageType()
    {
        return LoginMessage.MESSAGE_TYPE;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}
