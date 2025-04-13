using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

public class AvailableServerCommandMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 24111;

    private LogicCommand _command;

    public AvailableServerCommandMessage() : base(0)
    {
    }

    public override void Encode()
    {
        base.Encode();
        LogicCommandManager.EncodeCommand(this._stream, this._command);
    }

    public override short GetMessageType()
    {
        return AvailableServerCommandMessage.MESSAGE_TYPE;
    }

    public void SetServerCommand(LogicCommand command)
    {
        this._command = command;
    }
}
