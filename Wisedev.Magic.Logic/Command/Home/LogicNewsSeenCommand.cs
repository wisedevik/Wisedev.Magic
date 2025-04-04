using Wisedev.Magic.Logic.Command.Listener;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titam.DataStream;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(539)]
public class LogicNewsSeenCommand : LogicCommand
{
    private int _lastSeenNews;

    public override void Decode(ByteStream stream)
    {
        this._lastSeenNews = stream.ReadInt();
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        level.SetLastSeenNews(this._lastSeenNews);
        return 0;
    }

    public override int GetCommandType()
    {
        return 539;
    }
}
