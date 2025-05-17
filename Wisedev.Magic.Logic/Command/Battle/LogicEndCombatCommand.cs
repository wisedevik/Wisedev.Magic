using Wisedev.Magic.Logic.Level;

namespace Wisedev.Magic.Logic.Command.Battle;

[LogicCommand(COMMAND_TYPE)]
public class LogicEndCombatCommand : LogicCommand
{
    public const int COMMAND_TYPE = 603;

    public override int Execute(LogicLevel level)
    {
        if (!level.GetGameMode().IsInAttackPreparationMode())
        {
            level.EndBattle();
            return 0;
        }

        return -1;
    }

    public override int GetCommandType()
    {
        return COMMAND_TYPE;
    }
}
