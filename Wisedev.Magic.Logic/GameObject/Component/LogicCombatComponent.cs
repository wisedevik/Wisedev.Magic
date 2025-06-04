using Wisedev.Magic.Logic.GameObject.Component.Filter;

namespace Wisedev.Magic.Logic.GameObject.Component;

public class LogicCombatComponent : LogicComponent
{
    private LogicComponentFilter _componentFilter;

    public LogicCombatComponent(LogicGameObject gameObject) : base(gameObject)
    {
    }
}
