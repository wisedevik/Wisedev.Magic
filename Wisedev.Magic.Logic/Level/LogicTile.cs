using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Level;

public class LogicTile
{
    private LogicRect _rect;
    private List<LogicGameObject> _gameObjects;
    private int _x;
    private int _y;
    private byte[] _flags;

    private int _roomIdx;
    private int _pathFinderCost;
    private byte _passableFlag;

    public LogicTile(int x, int y, LogicRect rect)
    {
        this._x = x;
        this._y = y;
        this._rect = rect;
        this._roomIdx = -1;
        this._passableFlag = 1;
        this._pathFinderCost = 0;
        this._flags = [1, 1, 1, 1];

        this._gameObjects = new List<LogicGameObject>();
    }

    public void AddGameObject(LogicGameObject gameObject)
    {
        this._gameObjects.Add(gameObject);

        if (!gameObject.IsPassable())
            this._passableFlag = 0;

        this.RefreshSubTiles();
    }

    public bool IsPassable(LogicGameObject gameObject)
    {
        for (int i = 0; i < this._gameObjects.Count; i++)
        {
            if (!this._gameObjects[i].IsPassable())
            {
                return this._gameObjects[i] == gameObject;
            }
        }

        return true;
    }

    public bool IsBuildable(LogicGameObject gameObject)
    {
        for (int i = 0; i < this._gameObjects.Count; i++)
        {
            LogicGameObject gO = this._gameObjects[i];

            if (gO != gameObject)
            {
                if (!gO.IsPassable() || gO.IsUnbuildable())
                    return false;
            }
        }

        return true;
    }

    public bool IsBuildableWithIgnoreList(LogicGameObject[] gameObjects, int cnt)
    {
        for (int i = 0, index = -1; i < this._gameObjects.Count; i++, index = -1)
        {
            LogicGameObject gO = this._gameObjects[i];

            for (int k = 0; k < cnt; k++)
            {
                if (gameObjects[i] == gO)
                {
                    index = k;
                    break;
                }
            }

            if (index == -1)
            {
                if (!this._gameObjects[i].IsPassable() || this._gameObjects[i].IsUnbuildable())
                    return false;
            }
        }

        return true;
    }

    public void RemoveGameObject(LogicGameObject gameObject)
    {
        int index = -1;

        for (int i = 0; i < this._gameObjects.Count; i++)
        {
            if (this._gameObjects[i] == gameObject)
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            this._gameObjects.RemoveAt(index);
            this.RefreshPassableFlag();
        }
    }

    public void RefreshPassableFlag()
    {
        byte flag = 1;

        for (int i = 0; i < this._gameObjects.Count; i++)
        {
            if (!this._gameObjects[i].IsPassable())
                flag = 0;
        }

        this._passableFlag = flag;
        this.RefreshSubTiles();
    }

    public void SetRoomIdx(int roomIdx)
    {
        this._roomIdx = roomIdx;
    }

    public int IsPassablePathFinder(uint x, uint y)
    {
        if (x > 1 || y > 1)
            return 0;

        int index = (int)(x + 2 * y);
        return _flags[index] != 0 ? 1 : 0;
    }

    public int IsFullyNotPassable()
    {
        for (int i = 0; i <= 3; i++)
        {
            if (_flags[i] != 0)
                return 0;
        }
        return 1;
    }

    public int GetPathFinderCost(int x, int y)
    {
        int result = this._pathFinderCost;

        if (result <= 0)
        {
            bool condition = x > 1;

            result = int.MaxValue;
            if (x <= 1)
                condition = y > 1;

            if (!condition)
            {
                result = int.MaxValue;

                if (this._flags[x + 2 * y] != 0)
                    return 0;
            }
        }

        return result;
    }

    public bool HasWall()
    {
        for (int i = 0; i < this._gameObjects.Count(); i++)
        {
            LogicGameObject gameObject = this._gameObjects[i];

            if (gameObject.IsWall() && gameObject.IsAlive())
            {
                return true;
            }
        }

        return false;
    }

    public void RefreshSubTiles()
    {
        this._flags[0] = 1;
        this._flags[1] = 1;
        this._flags[2] = 1;
        this._flags[3] = 1;

        this._pathFinderCost = 0;

        for (int i = 0; i < this._gameObjects.Count; i++)
        {
            LogicGameObject gameObject = this._gameObjects[i];
            this._pathFinderCost = Math.Max(gameObject.PathFinderCost(), this._pathFinderCost);

            if (!gameObject.IsPassable())
            {
                int width = gameObject.GetWidthInTiles();
                int heigh = gameObject.GetWidthInTiles();

                if (width == 1 || heigh == 1)
                {
                    this._flags[0] = 0;
                    this._flags[1] = 0;
                    this._flags[2] = 0;
                    this._flags[3] = 0;
                }
                else
                {
                    int edge = gameObject.PassableSubtilesAtEdge();

                    this._rect.SetStartX(edge);
                    this._rect.SetStartY(edge);
                    this._rect.SetEndX(2 * width - edge);
                    this._rect.SetEndY(2 * heigh - edge);

                    int endX = 2 * (width - edge);
                    int endY = 2 * (heigh - edge);

                    if (_rect.IsInside(endX, endY))
                        this._flags[0] = 0;
                    if (this._rect.IsInside(endX, endY | 1))
                        this._flags[2] = 0;
                    if (this._rect.IsInside(endX | 1, endY))
                        this._flags[1] = 0;
                    if (this._rect.IsInside(endX | 1, endY | 1))
                        this._flags[3] = 0;
                }
            }
        }
    }
}
