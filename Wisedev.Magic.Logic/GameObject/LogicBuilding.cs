using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Level;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicBuilding : LogicGameObject
{
    public LogicBuilding(LogicBuildingData data, LogicLevel level) : base(data, level)
    {
    }

    public override int GetGameObjectType()
    {
        return 0;
    }
}
