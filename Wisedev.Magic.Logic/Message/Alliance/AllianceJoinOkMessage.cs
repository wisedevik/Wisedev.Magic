using Wisedev.Magic.Logic.Command.Home;
using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Alliance;

public class AllianceJoinOkMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 24303;

    private LogicJoinAllianceCommand _command;

    public AllianceJoinOkMessage() : base(0)
    {
    }

    public override void Encode()
    {
        base.Encode();

    }

    public override short GetMessageType()
    {
        return AllianceJoinOkMessage.MESSAGE_TYPE;
    }
}
