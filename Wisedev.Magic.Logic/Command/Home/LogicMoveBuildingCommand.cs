using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(501)]
public class LogicMoveBuildingCommand : LogicCommand
{
    private int _x;
    private int _y;
    private int _gameObjectId;

    public override void Decode(ByteStream stream)
    {
        this._x = stream.ReadInt();
        this._y = stream.ReadInt();
        this._gameObjectId = stream.ReadInt();
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        LogicGameObject gameObject = level.GetGameObjectManager().GetGameObjectByID(this._gameObjectId);

        if (gameObject != null)
        {
            int gameObjectType = gameObject.GetGameObjectType();

            if (gameObjectType == 0 || gameObjectType == 4 || gameObjectType == 6)
            {
                int x = gameObject.GetTileX();
                int y = gameObject.GetTileY();
                int width = gameObject.GetWidthInTiles();
                int height = gameObject.GetHeightInTiles();

                if (level.IsValidPlaceForBuilding(this._x, this._y, width, height, gameObject))
                {
                    gameObject.SetPositionXY(this._x << 9, this._y << 9);
                    Debugger.Print($"New {gameObject.GetData().GetGlobalID()} position is x: {this._x} ({this._x << 9}) y: {this._y} ({this._y << 9})");
                    return 0;
                }

                return -3;
            }

            return -2;
        }

        return -1;
    }
}
