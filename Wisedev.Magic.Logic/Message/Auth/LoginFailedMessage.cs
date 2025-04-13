using System.IO;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Auth;

public class LoginFailedMessage : PiranhaMessage
{
    private ErrorCode _errorCode { get; set; }
    private string _resourceFingerprintData { get; set; }
    private string _redirectDomain { get; set; }
    private string _contentURL { get; set; }
    private string _updateURL { get; set; }
    private string _reason { get; set; }

    public LoginFailedMessage() : base(0)
    {
        _errorCode = 0;
        _resourceFingerprintData = string.Empty;
        _redirectDomain = string.Empty;
        _contentURL = string.Empty;
        _updateURL = string.Empty;
        _reason = string.Empty;
    }

    public override void Encode()
    {
        base.Encode();
        _stream.WriteInt((int)_errorCode);
        _stream.WriteString(_resourceFingerprintData);
        _stream.WriteString(_redirectDomain);
        _stream.WriteString(_contentURL);
        _stream.WriteString(_updateURL);
        _stream.WriteString(_reason);
    }

    public void SetErrorCode(ErrorCode eC)
    {
        this._errorCode = eC;
    }

    public void SetResourceFingerprintData(string rFD)
    {
        this._resourceFingerprintData = rFD;
    }

    public void SetRedirectDomain(string rD)
    {
        this._redirectDomain = rD;
    }

    public void SetContentURL(string cU)
    {
        this._contentURL = cU;
    }

    public void SetUpdateURL(string uU)
    {
        this._updateURL = uU;
    }

    public void SetReason(string r)
    {
        this._reason = r;
    }

    public enum ErrorCode
    {
        DATA_VERSION = 7,
        CLIENT_VERSION = 8,
        SERVER_MAINTENANCE = 10,
    }

    public override short GetMessageType()
    {
        return 20103;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}
