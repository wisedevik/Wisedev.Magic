using Wisedev.Magic.Titam.Logic;
using Wisedev.Magic.Titam.Message;

namespace Wisedev.Magic.Logic.Message.Auth;

public class LoginOkMessage : PiranhaMessage
{
    private LogicLong _accountId;
    private LogicLong _homeId;
    private string? _passToken;
    private string? _facebookId;
    private string? _gamecenterId;
    private int _majorVersion;
    private int _build;
    private int _contentVersion;
    private string? _environment;
    private int _sessionCount;
    private int _playTimeSeconds;
    private int _daysSinceStartedPlaying;
    private string? _facebookAppId;
    private string? _serverTime;
    private string? _accountCreatedDate;
    private int _startupCooldownSeconds;
    private string? _googleServiceId;

    public LoginOkMessage() : base(0)
    {
        this._accountId = new LogicLong();
        this._homeId = new LogicLong();
        this._passToken = null;
        this._facebookId = null;
        this._gamecenterId = null;
        this._majorVersion = 0;
        this._build = 0;
        this._contentVersion = 0;
        this._environment = string.Empty;
        this._sessionCount = 0;
        this._playTimeSeconds = 0;
        this._daysSinceStartedPlaying = 0;
        this._facebookAppId = null;
        this._serverTime = null;
        this._accountCreatedDate = null;
        this._startupCooldownSeconds = 0;
        this._googleServiceId = null;
    }

    public override void Encode()
    {
        base.Encode();
        this._stream.WriteLong(this._accountId);
        this._stream.WriteLong(this._homeId);
        this._stream.WriteString(this._passToken);
        this._stream.WriteString(this._facebookId);
        this._stream.WriteString(this._gamecenterId);
        this._stream.WriteInt(this._majorVersion);
        this._stream.WriteInt(this._build);
        this._stream.WriteInt(this._contentVersion);
        this._stream.WriteString(this._environment);
        this._stream.WriteInt(this._sessionCount);
        this._stream.WriteInt(this._playTimeSeconds);
        this._stream.WriteInt(this._daysSinceStartedPlaying);
        this._stream.WriteString(this._facebookAppId);
        this._stream.WriteString(this._serverTime);
        this._stream.WriteString(this._accountCreatedDate);
        this._stream.WriteInt(this._startupCooldownSeconds);
        this._stream.WriteString(this._googleServiceId);
    }

    public override short GetMessageType()
    {
        return 20104;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }

    public void SetAccountId(LogicLong value)
    {
        this._accountId = value;
    }

    public LogicLong GetAccountId()
    {
        return this._accountId;
    }

    public void SetHomeId(LogicLong value)
    {
        this._homeId = value;
    }

    public LogicLong GetHomeId()
    {
        return this._homeId;
    }

    public void SetPassToken(string? value)
    {
        this._passToken = value;
    }

    public string? GetPassToken()
    {
        return this._passToken;
    }

    public void SetFacebookId(string? value)
    {
        this._facebookId = value;
    }

    public string? GetFacebookId()
    {
        return this._facebookId;
    }

    public void SetGamecenterId(string? value)
    {
        this._gamecenterId = value;
    }

    public string? GetGamecenterId()
    {
        return this._gamecenterId;
    }

    public void SetMajorVersion(int value)
    {
        this._majorVersion = value;
    }

    public int GetMajorVersion()
    {
        return this._majorVersion;
    }

    public void SetBuild(int value)
    {
        this._build = value;
    }

    public int GetBuild()
    {
        return this._build;
    }

    public void SetContentVersion(int value)
    {
        this._contentVersion = value;
    }

    public int GetContentVersion()
    {
        return this._contentVersion;
    }

    public void SetEnvironment(string? value)
    {
        this._environment = value;
    }

    public string? GetEnvironment()
    {
        return this._environment;
    }

    public void SetSessionCount(int value)
    {
        this._sessionCount = value;
    }

    public int GetSessionCount()
    {
        return this._sessionCount;
    }

    public void SetPlayTimeSeconds(int value)
    {
        this._playTimeSeconds = value;
    }

    public int GetPlayTimeSeconds()
    {
        return this._playTimeSeconds;
    }

    public void SetDaysSinceStartedPlaying(int value)
    {
        this._daysSinceStartedPlaying = value;
    }

    public int GetDaysSinceStartedPlaying()
    {
        return this._daysSinceStartedPlaying;
    }

    public void SetFacebookAppId(string? value)
    {
        this._facebookAppId = value;
    }

    public string? GetFacebookAppId()
    {
        return this._facebookAppId;
    }

    public void SetServerTime(string? value)
    {
        this._serverTime = value;
    }

    public string? GetServerTime()
    {
        return this._serverTime;
    }

    public void SetAccountCreatedDate(string? value)
    {
        this._accountCreatedDate = value;
    }

    public string? GetAccountCreatedDate()
    {
        return this._accountCreatedDate;
    }

    public void SetStartupCooldownSeconds(int value)
    {
        this._startupCooldownSeconds = value;
    }

    public int GetStartupCooldownSeconds()
    {
        return this._startupCooldownSeconds;
    }

    public void SetGoogleServiceId(string? value)
    {
        this._googleServiceId = value;
    }

    public string? GetGoogleServiceId()
    {
        return this._googleServiceId;
    }

}
