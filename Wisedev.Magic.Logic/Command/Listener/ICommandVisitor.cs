using Wisedev.Magic.Logic.Command.Home;

namespace Wisedev.Magic.Logic.Command.Listener;

public interface ICommandVisitor
{
    void Visit(LogicNewsSeenCommand command);
}
