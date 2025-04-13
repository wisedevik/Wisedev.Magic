using System.Runtime.CompilerServices;
using Wisedev.Magic.Logic.GameObject;

namespace Wisedev.Magic.Logic.Level;

public class LogicTileMap
{
    private int _x;
    private int _y;

    private LogicTile[] _tiles;

    private bool _roomEnabled;

    public LogicTileMap(int x, int y)
    {
        this._x = x;
        this._y = y;
        this._tiles = new LogicTile[x * y];

        for (int i = 0; i < this._tiles.Length; i++)
        {
            this._tiles[i] = new LogicTile((byte)(i % x), (byte)(i / x), new LogicRect(0, 0, 0, 0));
        }
    }

    public void GameObjectMoved(LogicGameObject gameObject, int x, int y)
    {

    }

    public void EnableRoomIndices(bool enable)
    {
        this._roomEnabled = enable;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public LogicTile GetTile(int x, int y)
    {
        if (this._x > (uint)x && this._y > (uint)y)
            return this._tiles[x + this._x * y];
        return null;
    }
}
