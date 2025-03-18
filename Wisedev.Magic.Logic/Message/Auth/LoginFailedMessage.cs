using System.IO;
using Wisedev.Magic.Titam.Message;

namespace Wisedev.Magic.Logic.Message.Auth;

public class LoginFailedMessage : PiranhaMessage
{
    public int ErrorCode { get; set; }
    public string ResourceFingerprintData { get; set; }
    public string RedirectDomain { get; set; }
    public string ContentURL { get; set; }
    public string UpdateURL { get; set; }
    public string Reason { get; set; }

    public LoginFailedMessage() : base(0)
    {
        ErrorCode = 0;
        ResourceFingerprintData = string.Empty;
        RedirectDomain = string.Empty;
        ContentURL = string.Empty;
        UpdateURL = string.Empty;
        Reason = string.Empty;
    }

    public override void Encode()
    {
        base.Encode();
        _stream.WriteInt(ErrorCode);
        _stream.WriteString(ResourceFingerprintData);
        _stream.WriteString(RedirectDomain);
        _stream.WriteString(ContentURL);
        _stream.WriteString(UpdateURL);
        _stream.WriteString(Reason);
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
