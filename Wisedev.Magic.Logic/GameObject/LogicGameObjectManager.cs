using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.GameObject.Component;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titam.JSON;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicGameObjectManager
{
    private LogicLevel _level;
    private LogicComponentManager _componentManager;

    private List<LogicGameObject> _gameObjects;

    public LogicGameObjectManager(LogicLevel level)
    {
        this._level = level;
        this._componentManager = new LogicComponentManager(level);
        this._gameObjects = new List<LogicGameObject>(LogicGameObject.GAMEOBJECT_TYPE_COUNT);
    }

    public LogicComponentManager GetComponentManager()
    {
        return this._componentManager;
    }

    public void Save(LogicJSONObject logicJSONObject)
    {
        ; //TODO
    }

    public void Tick()
    {
        this._componentManager.Tick();

        for (int i = 0; i < LogicGameObject.GAMEOBJECT_TYPE_COUNT; i++)
        {
            for (int k = 0, size = this._gameObjects.Count; k < size; k++)
                this._gameObjects[i].Tick();
        }
    }
}
