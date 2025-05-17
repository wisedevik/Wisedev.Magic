using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

[PiranhaMessage(10117)]
public class ReportUserMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 10117;

    public int Count { get; set; }
    public LogicLong ReportedAvatarId { get; set; }

    public ReportUserMessage() : base(0)
    {
    }

    public override void Decode()
    {
        base.Decode();
        Count = _stream.ReadInt();
        ReportedAvatarId = _stream.ReadLong();
    }

    public override short GetMessageType()
    {
        return MESSAGE_TYPE;
    }
}
