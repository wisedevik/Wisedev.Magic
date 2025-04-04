using Wisedev.Magic.Logic.GameObject;

namespace Wisedev.Magic.Logic.Level;

public class LogicTileMap
{
    private int _x;
    private int _y;
    private bool _roomEnabled;

    public LogicTileMap(int x, int y)
    {
        this._x = x;
        this._y = y;
    }

    public void GameObjectMoved(LogicGameObject gameObject, int x, int y)
    {

    }

    public void EnableRoomIndices(bool enable)
    {
        this._roomEnabled = enable;
    }
}
