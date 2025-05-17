using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(COMMAND_TYPE)]
public class LogicUpgradeBuildingCommand : LogicCommand
{
    public const int COMMAND_TYPE = 502;

    private int _buildingId;

    public override void Decode(ByteStream stream)
    {
        _buildingId = stream.ReadInt();
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        LogicGameObject gameObject = level.GetGameObjectManager().GetGameObjectByID(_buildingId);
        if (gameObject != null)
        {
            if (gameObject.GetType() == 0)
            {
                LogicBuilding building = (LogicBuilding)gameObject;
                LogicBuildingData buildingData = building.GetBuildingData();
                if (building.CanUpgrade(true))
                {
                    int nextUpgradeLevel = building.GetUpgradeLevel() + 1;
                    int buildCost = buildingData.GetBuildCost(nextUpgradeLevel, level);

                    LogicResourceData buildResourceData = buildingData.GetBuildResource(nextUpgradeLevel);

                    LogicClientAvatar playerAvatar = level.GetPlayerAvatar();
                    if (playerAvatar.HasEnoughResources(buildResourceData, buildCost, true, this, false))
                    {
                        if (buildingData.GetConstructionTime(nextUpgradeLevel) != 0 && LogicDataTables.GetGlobals().WorkerForZeroBuildTime())
                        {
                            if (!level.HasFreeWorkers(this))
                            {
                                return -1;
                            }

                            playerAvatar.CommodityCountChangeHelper(0, buildResourceData, -buildCost);
                            building.StartUpgrading();
                            return 0;
                        }
                    }
                }
            }
        }

        return -2;
    }

    public override int GetCommandType()
    {
        return COMMAND_TYPE;
    }
}
