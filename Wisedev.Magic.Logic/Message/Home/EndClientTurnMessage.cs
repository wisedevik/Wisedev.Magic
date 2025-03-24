using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Titam.Message;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Message.Home;

[PiranhaMessage(EndClientTurnMessage.MESSAGE_TYPE)]
public class EndClientTurnMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 14102;

    private int _subTick;
    private int _checksum;
    private List<LogicCommand> _commands;

    public EndClientTurnMessage() : base(0)
    {
        this._commands = new List<LogicCommand>();
    }

    public override void Decode()
    {
        base.Decode();
        this._subTick = this._stream.ReadInt();
        this._checksum = this._stream.ReadInt();

        int cnt = this._stream.ReadInt();
        if (cnt <= 512)
        {
            if (cnt > 0)
            {
                this._commands = new List<LogicCommand>(cnt);

                do
                {
                    LogicCommand command = LogicCommandManager.DecodeCommand(this._stream);
                    if (command == null)
                        break;

                    this._commands.Add(command);
                } while (--cnt != 0);
            }
        }
        else
        {
            Debugger.Warning($"EndClientTurn.Decode() command count is too high! ({cnt})");
        }
    }

    public int GetSubTick()
    {
        return this._subTick;
    }

    public int GetChecksum()
    {
        return this._checksum;
    }

    public List<LogicCommand> GetCommands()
    {
        return this._commands;
    }

    public override short GetMessageType()
    {
        return EndClientTurnMessage.MESSAGE_TYPE;
    }
}
