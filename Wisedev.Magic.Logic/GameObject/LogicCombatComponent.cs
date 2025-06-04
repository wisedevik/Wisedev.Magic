using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Objects;
using Wisedev.Magic.Logic.GameObject.Component;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicCombatComponent : LogicComponent
{
    public LogicCombatComponent(LogicGameObject gameObject) : base(gameObject)
    {
    }

    public static bool IsPreferredTarget(LogicData data, LogicGameObject gameObject)
    {
        if (data != null && gameObject != null)
        {
            LogicData buildingClass;

            if (data.GetDataType() == LogicDataType.BUILDING_CLASS &&
                gameObject.GetData().GetDataType() == LogicDataType.BUILDING)
            {
                LogicBuildingData buildingData = (LogicBuildingData)gameObject.GetData();
                buildingClass = buildingData.GetBuildingClass();
            }
            else
            {
                buildingClass = gameObject.GetData();
            }

            return buildingClass == data;
        }

        return false;
    }
}
