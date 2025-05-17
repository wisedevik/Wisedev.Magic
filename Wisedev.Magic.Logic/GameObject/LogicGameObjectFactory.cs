using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.GameObject;

public static class LogicGameObjectFactory
{
    public static LogicGameObject? CreateGameObject(LogicData data, LogicLevel level)
    {
        LogicGameObject gameObject = null;

        switch (data.GetDataType())
        {
            case LogicDataType.BUILDING:
                gameObject = new LogicBuilding((LogicBuildingData)data, level); 
                break;
            case LogicDataType.OBSTACLE:
                gameObject = new LogicObstacle((LogicObstacleData)data, level);
                break;
            case LogicDataType.DECO:
                gameObject = new LogicDeco((LogicDecoData)data, level);
                break;
            case LogicDataType.SPELL:
                gameObject = new LogicSpell((LogicSpellData)data, level);
                break;
            default:
                Debugger.Error($"LogicGameObjectFactory.CreateGameObject: Unknown data ({data.GetDataType()})");
                break;
        }

        return gameObject;
    }
}
