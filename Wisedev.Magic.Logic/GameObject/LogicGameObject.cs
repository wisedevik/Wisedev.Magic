using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject.Component;
using Wisedev.Magic.Logic.GameObject.Listener;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Math;
using Wisedev.Magic.Titan.Debug;
using Wisedev.Magic.Logic.Avatar;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicGameObject
{
    public const int GAMEOBJECT_TYPE_COUNT = 11;

    protected int _globalId;
    protected LogicData _data;
    protected LogicLevel _level;
    protected LogicComponent[] _components;

    protected LogicVector2 _position;
    protected LogicGameObjectListener _listener;

    private int _freezeDelay;
    private int _freezeTime;

    public LogicGameObject(LogicData data, LogicLevel level)
    {
        this._listener = new LogicGameObjectListener();
        this._data = data;
        this._level = level;
        this._position = new LogicVector2();

        this._components = new LogicComponent[LogicComponent.COMPONENT_TYPE_COUNT];
    }

    public virtual void SetInitialPosition(int x, int y)
    {
        this._position.Set(x, y);
        this._listener.RefreshPositionFromLogic();
    }

    public void SetGlobalID(int id)
    {
        this._globalId = id;
    }

    public void XpGainHelper(int xp, LogicAvatar homeOwnerAvatar, bool inHomeState)
    {
        LogicClientAvatar playerAvatar = this._level.GetPlayerAvatar();
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

    public virtual int GetType()
    {
        return -1;
    }

    public bool IsWall()
    {
        return false;
    }

    public virtual int GetGameObjectType()
    {
        return 0;
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

    public virtual int GetHeightInTiles()
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

    public int GetX()
    {
        return this._position.GetX();
    }

    public int GetY()
    {
        return this._position.GetY();
    }

    public virtual int GetMidX()
    {
        return this._position.GetX() + (this.GetWidthInTiles() << 8);
    }

    public virtual int GetMidY()
    {
        return this._position.GetY() + (this.GetHeightInTiles() << 8);
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

    public void EnableComponent(int type, bool enabled)
    {
        LogicComponent component = this._components[type];

        if (component != null)
        {
            component.SetEnabled(enabled);
        }
    }

    public virtual void Save(LogicJSONObject logicJSON)
    {
        logicJSON.Put("x", new LogicJSONNumber(this.GetTileX()));
        logicJSON.Put("y", new LogicJSONNumber(this.GetTileY()));

        for (int i = 0; i < this._components.Length; i++)
        {
            LogicComponent component = this._components[i];

            if (component != null)
                component.Save(logicJSON);
        }
    }

    public virtual void Load(LogicJSONObject logicJSON)
    {
        int x = logicJSON.GetJSONNumber("x").GetIntValue();
        int y = logicJSON.GetJSONNumber("y").GetIntValue();

        for (int i = 0; i < this._components.Length; i++)
        {
            LogicComponent component = this._components[i];
            if (component != null)
                component.Load(logicJSON);
        }

        this.SetInitialPosition(x << 9, y << 9);
    }

    public LogicLevel GetLevel()
    {
        return this._level;
    }
}
