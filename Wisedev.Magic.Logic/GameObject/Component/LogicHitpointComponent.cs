namespace Wisedev.Magic.Logic.GameObject.Component;

public class LogicHitpointComponent : LogicComponent
{
    private int _maxRegenerationTime;
    private bool _regenerationEnabled;
    private int _team;
    private int _hitpoints;
    private int _maxHitpoints;

    public LogicHitpointComponent(LogicGameObject gameObject, int hitpoints, int team) : base(gameObject)
    {
        _team = team;

        _hitpoints = 100 * hitpoints;
        _maxHitpoints = 100 * hitpoints;
    }



}
