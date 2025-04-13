using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(512)]
public class LogicBuyDecoCommand : LogicCommand
{
    private int _x;
    private int _y;
    private LogicDecoData _decoData;

    public override void Decode(ByteStream stream)
    {
        this._x = stream.ReadInt();
        this._y = stream.ReadInt();
        this._decoData = (LogicDecoData)ByteStreamHelper.ReadDataReference(stream, LogicDataType.DECO);
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        if (this._decoData != null)
        {
            if (level.IsValidPlaceForBuilding(this._x, this._y, this._decoData.GetWidth(), this._decoData.GetHeight(), null))
            {
                LogicClientAvatar playerAvatar = level.GetPlayerAvatar();
                LogicResourceData buildResourceData = this._decoData.GetBuildResource();

                int buildCost = this._decoData.GetBuildCost();

                Debugger.Print($"{playerAvatar.HasEnoughResources(buildResourceData, buildCost, true, this, false)}");
                if (playerAvatar.HasEnoughResources(buildResourceData, buildCost, true, this, false)) // TODO: ADD LogicLevel::isDecoCapReached {
                {
                    if (buildResourceData.IsPremiumCurrency())
                    {
                        playerAvatar.UseDiamonds(buildCost);
                    }
                    else
                    {
                        Debugger.Print($"{buildCost} : {-buildCost}");
                        playerAvatar.CommodityCountChangeHelper(0, buildResourceData, -buildCost);
                    }

                    LogicDeco? deco = (LogicDeco)LogicGameObjectFactory.CreateGameObject(this._decoData, level);
                    if (deco != null)
                    {
                        deco.SetInitialPosition(this._x << 9, this._y << 9);
                        level.GetGameObjectManager().AddGameObject(deco, -1);
                    }

                    Debugger.Print($"New object at {this._x}:{this._y}");

                    return base.Execute(level);
                }

                return -1;
            }

            return -2;
        }

        return -3;

    }

    public override int GetCommandType()
    {
        return 512;
    }
}
