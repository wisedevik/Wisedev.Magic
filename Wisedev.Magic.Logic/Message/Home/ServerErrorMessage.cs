using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

public class ServerErrorMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 24115;

    public string ErrorMessage { get; set; }

    public ServerErrorMessage() : base(0)
    {
    }

    public override void Encode()
    {
        base.Encode();
        _stream.WriteString(ErrorMessage);
    }

    public override int GetServiceNodeType()
    {
        return MESSAGE_TYPE;
    }

}
