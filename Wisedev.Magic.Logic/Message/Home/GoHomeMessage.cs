using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

[PiranhaMessage(GoHomeMessage.MESSAGE_TYPE)]
public class GoHomeMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 14101;

    public GoHomeMessage() : base(0)
    {
    }

    public override void Decode()
    {
        base.Decode();
    }

    public override short GetMessageType()
    {
        return GoHomeMessage.MESSAGE_TYPE;
    }
}
