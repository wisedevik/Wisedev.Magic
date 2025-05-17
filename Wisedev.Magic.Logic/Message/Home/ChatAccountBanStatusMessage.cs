using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

public class ChatAccountBanStatusMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 20118;

    public int BanTime { get; set; }

    public ChatAccountBanStatusMessage() : base(0)
    {
    }

    public override void Encode()
    {
        base.Encode();
        _stream.WriteInt(BanTime);
    }

    public override short GetMessageType()
    {
        return MESSAGE_TYPE;
    }
}
