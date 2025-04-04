using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Level;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicObstacle : LogicGameObject
{
    public LogicObstacle(LogicObstacleData data, LogicLevel level) : base(data, level)
    {
    }

    public bool IsFadingOut()
    {
        return false;
    }

    public override int GetGameObjectType()
    {
        return 3;
    }
}
