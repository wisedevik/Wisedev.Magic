using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Worker;

public class LogicWorkerManager
{
    private LogicLevel _level;
    private int _totalWorkers;
    private List<LogicGameObject> _buildings;

    public LogicWorkerManager(LogicLevel level)
    {
        this._level = level;
        this._totalWorkers = 0;
        this._buildings = new List<LogicGameObject>();
    }

    public int GetTotalWorkers()
    {
        return this._totalWorkers;
    }

    public int GetFreeWorkers()
    {
        return this._totalWorkers - this._buildings.Count;
    }

    public void AllocateWorker(LogicGameObject gameObject)
    {
        if (this._buildings.IndexOf(gameObject) != -1)
        {
            Debugger.Warning("LogicWorkerManager.AllocateWorker called twice for same target!");
            return;
        }

        this._buildings.Add(gameObject);
    }

    public void DeallocateWorker(LogicGameObject gameObject)
    {
        int index = this._buildings.IndexOf(gameObject);

        if (index != -1)
        {
            this._buildings.RemoveAt(index);
        }
    }

    public void RemoveGameObjectReferences(LogicGameObject gameObject)
    {
        this.DeallocateWorker(gameObject);
    }

    public void IncreaseWorkerCount()
    {
        ++this._totalWorkers;
    }

    public void DecreaseWorkerCount()
    {
        if (this._totalWorkers-- <= 0)
            Debugger.Error("LogicWorkerManager - Total worker count below 0");
    }

    public LogicGameObject GetShortestTaskGO()
    {
        LogicGameObject gameObject = null;

        for (int i = 0; i < this._buildings.Count; i++)
        {
            LogicGameObject tmp = this._buildings[i];

        }

        return null; // TODO: MAKE
    }
}
