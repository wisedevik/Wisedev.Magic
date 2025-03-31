using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject.Component;
using Wisedev.Magic.Logic.GameObject.Listener;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titam.Math;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicGameObject
{
    public const int GAMEOBJECT_TYPE_COUNT = 8;

    protected int _globalId;
    protected LogicData _data;
    protected LogicLevel _level;
    protected List<LogicComponent> _components;

    protected LogicVector2 _position;
    protected LogicGameObjectListener _listener;

    private int _freezeDelay;
    private int _freezeTime;

    public LogicGameObject(LogicData data, LogicLevel level)
    {
        this._listener = new LogicGameObjectListener();
        this._data = data;
        this._level = level;
        this._components = new List<LogicComponent>(LogicComponent.COMPONENT_TYPE_COUNT);

        this._position = new LogicVector2();
    }

    public void SetInitialPosition(int x, int y)
    {
        this._position.Set(x, y);
        this._listener.RefreshPositionFromLogic();
    }

    public LogicData GetData()
    {
        return this._data;
    }

    public virtual bool IsAlive()
    {
        return true; // TODO LOGIC!!!
    }


    public virtual bool IsUnbuildable()
    {
        return false;
    }

    public virtual int PathFinderCost()
    {
        return 0;
    }

    public bool IsWall()
    {
        return false;
    }

    public virtual bool IsHero()
    {
        return false;
    }

    public virtual bool IsPassable()
    {
        return true;
    }

    public virtual LogicGameObjectListener GetListener()
    {
        return this._listener;
    }

    public virtual bool IsHidden()
    {
        return false;
    }

    public virtual int GetWidthInTiles()
    {
        return 1;
    }

    public virtual int PassableSubtilesAtEdge()
    {
        return 1;
    }

    public virtual bool IsStaticObject()
    {
        return true;
    }

    public virtual int GetDirection()
    {
        return 0;
    }

    public virtual int GetRemainingBoostTime()
    {
        return 0;
    }

    public virtual int GetMaxFastForwardTime()
    {
        return -1;
    }

    public virtual bool ShouldDestruct()
    {
        return false;
    }

    public int GetGlobalID()
    {
        return this._globalId;
    }

    public virtual void Tick()
    {
        if (this._freezeTime < 1 || this._freezeDelay != 0)
        {
            if (this._freezeDelay >= 1)
            {
                this._freezeDelay -= 1;
            }
        }
        else
        {
            this._freezeTime -= 1;
        }
    }

    public virtual void SubTick()
    {
        ;
    }

    public virtual void SetPosition(LogicVector2 pos)
    {
        this.SetPositionXY(pos.GetX(), pos.GetY());
    }

    public virtual int GetTileX()
    {
        return this._position.GetX() >> 9;
    }

    public virtual int GetTileY()
    {
        return this._position.GetY() >> 9;
    }

    public virtual void SetPositionXY(int x, int y)
    {
        if (this._position.GetX() != x || this._position.GetY() != y)
        {
            int tileX = this.GetTileX();
            int tileY = this.GetTileY();

            this._position.Set(x, y);

            if (this._globalId != 0)
                this._level.GetTileMap().GameObjectMoved(this, tileX, tileY);
        }
    }

    public void AddComponent(LogicComponent component)
    {
        int type = component.GetComponentType();

        if (this._components[type] != null)
        {
            Debugger.Error($"LogicGameObject.addComponent - Component (name={component.GetType().Name}, type={component.GetComponentType()}) is already added.");
        }
        else
        {
            this._level.GetComponentManager().AddComponent(component);
            this._components[type] = component;
        }
    }

    public void RemoveComponent(int type)
    {
        if (_components[type] != null)
        {
            this._components[type].Destruct();
            this._components[type] = null;
        }
        else
        {
            Debugger.Error($"LogicGameObject.RemoveComponent: component with type={type} is NULL!");
        }
    }

    public LogicComponent? GetComponent(int type)
    {
        LogicComponent component = this._components[type];

        if (component != null && component.IsEnabled())
        {
            return component;
        }

        return null;
    }
}
