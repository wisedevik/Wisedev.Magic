using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject.Component;
using Wisedev.Magic.Logic.GameObject.Listener;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titam.Math;

namespace Wisedev.Magic.Logic.GameObject;

class LogicGameObject
{
    protected LogicData _data;
    protected LogicLevel _level;
    protected List<LogicComponent> _components;

    protected LogicVector2 _position;
    protected LogicGameObjectListener _listener;

    public LogicGameObject(LogicData data, LogicLevel level)
    {
        this._data = data;
        this._level = level;

        this._position = new LogicVector2();
    }

    public void SetInitialPosition(int x, int y)
    {
        this._position.Set(x, y);
        _listener.RefreshPositionFromLogic();
    }
}
