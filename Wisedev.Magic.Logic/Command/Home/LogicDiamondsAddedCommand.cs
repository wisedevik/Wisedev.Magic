using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Command.Home;

public class LogicDiamondsAddedCommand : LogicServerCommand
{
    public int Amount { get; set; }

    public const int COMMAND_TYPE = 7;

    public override void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteBoolean(true); // free;
        encoder.WriteInt(Amount); // amount;
        encoder.WriteString("trans:id"); // transactionId;
        base.Encode(encoder);
    }

    public override int GetCommandType()
    {
        return COMMAND_TYPE;
    }
}
