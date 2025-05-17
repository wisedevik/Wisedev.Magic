using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.GameObject.Component;
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

        int minRemaining = 0;
        for (int i = 0; i < this._buildings.Count; i++)
        {
            LogicGameObject tmp = this._buildings[i];
            int remaining = 0;

            switch (tmp.GetType())
            {
                case 3:
                    LogicObstacle obstacle = (LogicObstacle)tmp;

                    if (!obstacle.IsClearingOnGoing())
                    {
                        Console.WriteLine("LogicWorkerManager - Worker allocated to obstacle with remaining clearing time 0");
                    }
                    else 
                    {
                        remaining = obstacle.GetRemainingClearingTime();
                    }
                    break;
                case 0:
                    LogicBuilding building = (LogicBuilding)tmp;

                    LogicHeroBaseComponent heroBaseComponent = building.GetHeroBaseComponent();

                    if (building.IsConstructing())
                    {
                        remaining = building.GetRemainingConstructionTime();
                    }
                    else
                    {
                        if (heroBaseComponent == null)
                        {
                            Console.WriteLine("LogicWorkerManager - Worker allocated to building with remaining construction time 0");
                        }
                        else
                        {
                            if (heroBaseComponent.IsUpgrading())
                            {
                                remaining = heroBaseComponent.GetRemainingUpgradeSeconds();
                            }
                            else
                            {
                                Console.WriteLine("LogicWorkerManager - Worker allocated to altar/herobase without hero upgrading");
                            }
                        }
                    }
                    break;
            }

            if (remaining < minRemaining || gameObject == null)
            {
                gameObject = tmp;
                minRemaining = remaining;
            }
        }

        return gameObject;
    }

    public bool FinishTaskOfOneWorker()
    {
        LogicGameObject gameObject = this.GetShortestTaskGO();

        if (gameObject != null)
        {
            switch (gameObject.GetType())
            {
                case 0:
                    LogicBuilding building = (LogicBuilding)gameObject;

                    if (building.IsConstructing())
                    {
                        return building.SpeedUpConstruction();
                    }

                    if (building.GetHeroBaseComponent() != null)
                    {
                        return building.GetHeroBaseComponent().SpeedUpUpgrade();
                    }

                    break;
                case 3:
                    LogicObstacle obstacle = (LogicObstacle)gameObject;

                    if (obstacle.IsClearingOnGoing())
                    {
                        return obstacle.SpeedUpClearing();
                    }
                    break;

            }
        }

        return false;
    }
}
