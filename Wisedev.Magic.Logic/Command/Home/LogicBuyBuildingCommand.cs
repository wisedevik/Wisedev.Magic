using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(COMMAND_TYPE)]
public class LogicBuyBuildingCommand : LogicCommand
{
    public const int COMMAND_TYPE = 500;

    private int _x;
    private int _y;
    private LogicBuildingData _data;

    public override void Decode(ByteStream stream)
    {
        _x = stream.ReadInt();
        _y = stream.ReadInt();
        _data = (LogicBuildingData)ByteStreamHelper.ReadDataReference(stream);
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        if (_data.GetBuildingClass().CanBuy())
        {
            if (level.IsValidPlaceForBuilding(_x, _y, _data.GetWidth(), _data.GetHeight(), null))
            {
                LogicResourceData buildResource = _data.GetBuildResource(0);
                int buildCost = _data.GetBuildCost(0, level);
                LogicClientAvatar avatar = level.GetPlayerAvatar();

                if (avatar.HasEnoughResources(buildResource, buildCost, true, this, false))
                {
                    if (_data.IsWorkerBuilding() ||
                        _data.GetConstructionTime(0) <= 0 && 
                        LogicDataTables.GetGlobals().WorkerForZeroBuildTime() ||
                        level.HasFreeWorkers(this))
                    {
                        if (buildResource.IsPremiumCurrency())
                        {
                            avatar.UseDiamonds(buildCost);
                        }
                        else
                        {
                            avatar.CommodityCountChangeHelper(0, buildResource, -buildCost);
                        }

                        LogicBuilding building = (LogicBuilding)LogicGameObjectFactory.CreateGameObject(this._data, level);
                        building.SetInitialPosition(this._x << 9, this._y << 9);
                        level.GetGameObjectManager().AddGameObject(building, -1);
                        building.StartConstructing();

                        return 0;
                    }
                }
            }
        }

        return -1;
    }

    public override int GetCommandType()
    {
        return COMMAND_TYPE;
    }
}
