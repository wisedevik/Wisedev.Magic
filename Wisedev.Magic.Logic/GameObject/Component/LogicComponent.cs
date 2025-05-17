using Wisedev.Magic.Titan.JSON;

namespace Wisedev.Magic.Logic.GameObject.Component;

public class LogicComponent
{
    public const int COMPONENT_TYPE_COUNT = 11; // idk 'bout that

    protected bool _enabled;
    protected LogicGameObject _gameObject;

    public LogicComponent(LogicGameObject gameObject)
    {
        this._gameObject = gameObject;
        this._enabled = true;
    }

    public virtual void SubTick()
    {
        ;
    }

    public virtual void Tick()
    {
        ;
    }

    public virtual void LoadingFinished()
    {
        ;
    }

    public virtual void SetEnabled(bool enabled)
    {
        _enabled = enabled;
    }

    public virtual int GetChecksum()
    {
        return 0;
    }

    public virtual void FastForwardTime(int t)
    {
        ;
    }

    public virtual void Destruct()
    {
        ;
    }

    public virtual int GetComponentType()
    {
        return 0;
    }

    public virtual bool IsEnabled()
    {
        return this._enabled;
    }

    public virtual void Save(LogicJSONObject jsonObject)
    {
        ;
    }

    public virtual void Load(LogicJSONObject jsonObject)
    {
        ;
    }
}
