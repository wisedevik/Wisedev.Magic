using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject.Component;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Utils;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicGameObjectManager
{
    private LogicLevel _level;
    private LogicComponentManager _componentManager;
    private readonly int[] _gameObjectIds;
    private LogicTileMap _tileMap;

    private LogicBuilding _townHall;

    private List<LogicGameObject>[] _gameObjects;

    public LogicGameObjectManager(LogicTileMap tileMap, LogicLevel level)
    {
        this._level = level;
        this._tileMap = tileMap;
        this._componentManager = new LogicComponentManager(level);

        this._gameObjects = new List<LogicGameObject>[LogicGameObject.GAMEOBJECT_TYPE_COUNT];

        for (int i = 0; i < LogicGameObject.GAMEOBJECT_TYPE_COUNT; i++)
        {
            this._gameObjects[i] = new List<LogicGameObject>(32);
        }

        this._gameObjectIds = new int[LogicGameObject.GAMEOBJECT_TYPE_COUNT];
    }

    public LogicComponentManager GetComponentManager()
    {
        return this._componentManager;
    }

    public List<LogicGameObject>[] GetGameObjects()
    {
        return this._gameObjects;
    }

    public LogicJSONArray SaveGameObjects(int gameObjectType)
    {
        List<LogicGameObject> gameObjects = this._gameObjects[(int)gameObjectType];
        LogicJSONArray jsonArray = new LogicJSONArray(gameObjects.Count);

        for (int i = 0, cnt = gameObjects.Count; i < cnt; i++)
        {
            LogicGameObject gameObject = gameObjects[i];
            LogicJSONObject jsonObject = new LogicJSONObject();

            jsonObject.Put("data", new LogicJSONNumber(gameObject.GetData().GetGlobalID()));
            gameObject.Save(jsonObject);
            jsonArray.Add(jsonObject);
        }

        return jsonArray;
    }

    public LogicJSONArray SaveObstacles()
    {
        List<LogicGameObject> gameObjects = this._gameObjects[3];
        LogicJSONArray jsonArray = new LogicJSONArray(gameObjects.Count);

        for (int i = 0, cnt = gameObjects.Count; i < cnt; i++)
        {
            LogicObstacle gameObject = (LogicObstacle)gameObjects[i];

            if (!gameObject.IsFadingOut())
            {
                LogicJSONObject jsonObject = new LogicJSONObject();

                jsonObject.Put("data", new LogicJSONNumber(gameObject.GetData().GetGlobalID()));
                gameObject.Save(jsonObject);
                jsonArray.Add(jsonObject);
            }
        }

        return jsonArray;
    }

    public void Save(LogicJSONObject logicJSONObject)
    {
        logicJSONObject.Put("buildings", this.SaveGameObjects(0));
        logicJSONObject.Put("obstacles", this.SaveObstacles());
        logicJSONObject.Put("traps", this.SaveGameObjects(4));
        logicJSONObject.Put("decos", this.SaveGameObjects(6));

        LogicJSONObject respawnObject = new LogicJSONObject();

        respawnObject.Put("secondsFromLastRespawn", new LogicJSONNumber(0));
        respawnObject.Put("respawnSeed", new LogicJSONNumber(1529463799));
        respawnObject.Put("obstacleClearCounter", new LogicJSONNumber(0));

        logicJSONObject.Put("respawnVars", respawnObject);
    }

    public void Tick()
    {
        this._componentManager.Tick();

        for(int i = 0; i < LogicGameObject.GAMEOBJECT_TYPE_COUNT; i++)
            {
            List<LogicGameObject> gameObjects = this._gameObjects[i];

            for (int j = 0, size = gameObjects.Count; j < size; j++)
            {
                gameObjects[j].Tick();
            }
        }
    }

    public int GenerateGameObjectGlobalID(LogicGameObject gameObject)
    {
        int type = gameObject.GetGameObjectType();

        if ((int)type >= LogicGameObject.GAMEOBJECT_TYPE_COUNT)
        {
            Debugger.Error("LogicGameObjectManager.GenerateGameObjectGlobalID(). Index is out of bounds.");
        }

        return GlobalID.CreateGlobalID((int)type + 500, this._gameObjectIds[(int)type]++);
    }

    public void AddGameObject(LogicGameObject gameObject, int globalId)
    {
        int gameObjectType = gameObject.GetGameObjectType();

        if (globalId == -1)
        {
            globalId = this.GenerateGameObjectGlobalID(gameObject);
        }
        else
        {
            int table = GlobalID.GetClassID(globalId);
            int idx = GlobalID.GetInstanceID(globalId);

            if (table - 500 != (int)gameObjectType)
            {
                Debugger.Error(string.Format("LogicGameObjectManager.AddGameObject with global ID {0}, doesn't have right index", globalId));
            }

            if (this.GetGameObjectByID(globalId) != null)
            {
                Debugger.Error(string.Format("LogicGameObjectManager::addGameObject with global ID {0}, global ID already taken", globalId));
            }

            if (this._gameObjectIds[(int)gameObjectType] <= idx)
            {
                this._gameObjectIds[(int)gameObjectType] = idx + 1;
            }
        }

        gameObject.SetGlobalID(globalId);

        if (gameObjectType == 0)
        {
            LogicBuilding building = (LogicBuilding)gameObject;
            LogicBuildingData buildingData = building.GetBuildingData();

            if (buildingData.IsTownHall())
            {
                _townHall = building;
            }

            if (buildingData.IsWorkerBuilding())
            {
                this._level.GetWorkerManager().IncreaseWorkerCount();
            }
        }
        else if (gameObjectType == 3)
        {
            LogicObstacle obstacleObject = (LogicObstacle)gameObject;

        }

        this._gameObjects[gameObject.GetGameObjectType()].Add(gameObject);
    }

    public LogicGameObject GetGameObjectByID(int globalId)
    {
        List<LogicGameObject> gameObjects = this._gameObjects[GlobalID.GetClassID(globalId) - 500];

        for (int i = 0, cnt = gameObjects.Count; i < cnt; i++)
        {
            LogicGameObject gameObject = gameObjects[i];

            if (gameObject.GetGlobalID() == globalId)
            {
                return gameObject;
            }
        }

        return null;
    }

    public LogicBuilding GetTownHall()
    {
        return _townHall;
    }

    public void LoadGameObjectsFromJSonArray(LogicJSONArray jsonArray)
    {
        for (int i = 0; i < jsonArray.Size(); i++)
        {
            LogicJSONObject jsonObject = (LogicJSONObject)jsonArray.Get(i);

            if (jsonObject != null)
            {
                LogicJSONNumber dataObject = jsonObject.GetJSONNumber("data");

                if (dataObject != null)
                {
                    LogicData? data = LogicDataTables.GetDataById(dataObject.GetIntValue());

                    if (data != null)
                    {
                        LogicDataType dataType = data.GetDataType();

                        if (dataType != LogicDataType.BUILDING &&
                            dataType != LogicDataType.OBSTACLE &&
                            dataType != LogicDataType.TRAP &&
                                    dataType != LogicDataType.DECO)
                            return;

                        LogicGameObject? gameObject = LogicGameObjectFactory.CreateGameObject(data, this._level);
                        if (gameObject != null)
                        {
                            gameObject.Load(jsonObject);
                            this.AddGameObject(gameObject, -1);
                        }
                    }

                }
            }
        }
    }

    public void AdjustConstructionTimers(int secondsPassed)
    {
        List<LogicGameObject> buildings = _gameObjects[0];
        foreach (LogicGameObject gameObject in buildings)
        {
            if (gameObject is LogicBuilding building && building.IsConstructing())
            {
                building.AdjustConstructionTime(secondsPassed);
            }
        }
    }

    public void Load(LogicJSONObject jsonObject)
    {
        LogicJSONArray buildingArray = jsonObject.GetJSONArray("buildings");
        LogicJSONArray obstaclesArray = jsonObject.GetJSONArray("obstacles");
        LogicJSONArray trapArray = jsonObject.GetJSONArray("traps");
        LogicJSONArray decoArray = jsonObject.GetJSONArray("decos");

        if (buildingArray != null)
        {
            Debugger.Print("LogicGameObjectManager.Load: buildingArray != null");
            this.LoadGameObjectsFromJSonArray(buildingArray);
        }
        else
        {
            Debugger.Error("LogicGameObjectManager.load - Building array is NULL!");
            return;
        }

        if (obstaclesArray != null)
        {
            Debugger.Print("LogicGameObjectManager.Load: obstaclesArray != null");
            this.LoadGameObjectsFromJSonArray(obstaclesArray);
            
        }
        else
        {
            Debugger.Error("LogicGameObjectManager.load - Obstacles array is NULL!");
            return;
        }

        if (trapArray != null)
        {
            Debugger.Print("LogicGameObjectManager.Load: trapArray != null");
            this.LoadGameObjectsFromJSonArray(trapArray);
        }
        else
        {
            Debugger.Print("LogicGameObjectManager.Load: Trap array == null");
        }

        if (decoArray != null)
        {
            Debugger.Print("LogicGameObjectManager.Load: decoArray != null");
            this.LoadGameObjectsFromJSonArray(decoArray);
        }
        else
        {
            Debugger.Print("LogicGameObjectManager.Load: Deco array == null");

        }

        LogicJSONObject respawnVarsObj = jsonObject.GetJSONObject("respawnVars");

        if (respawnVarsObj != null)
        {
            int secondsFromLastRespawn = respawnVarsObj.GetJSONNumber("secondsFromLastRespawn").GetIntValue();
            int respawnSeed = respawnVarsObj.GetJSONNumber("respawnSeed").GetIntValue();
            int obstacleClearCounter = respawnVarsObj.GetJSONNumber("obstacleClearCounter").GetIntValue();
        }

        this._tileMap.EnableRoomIndices(true);
    }
}
