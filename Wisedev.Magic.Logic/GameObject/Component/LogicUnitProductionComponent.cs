namespace Wisedev.Magic.Logic.GameObject.Component;

public class LogicUnitProductionComponent : LogicComponent
{
    public LogicUnitProductionComponent(LogicGameObject gameObject) : base(gameObject)
    {
    }

    public override int GetComponentType()
    {
        return 3;
    }
}
