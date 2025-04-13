using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(LogicCollectResourcesCommand.COMMAND_TYPE)]
public class LogicCollectResourcesCommand : LogicCommand
{
    public const int COMMAND_TYPE = 506;

    private int _objectId;

    public LogicCollectResourcesCommand() : base()
    {
        ;
    }

    public override void Decode(ByteStream stream)
    {
        this._objectId = stream.ReadInt();
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        Debugger.Print($"LogicCollectResourcesCommand.Execute: _objectId={this._objectId}");

        return 0;
    }

    public override int GetCommandType()
    {
        return LogicCollectResourcesCommand.COMMAND_TYPE;
    }
}
