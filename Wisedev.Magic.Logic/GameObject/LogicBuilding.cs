using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject.Component;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicBuilding : LogicGameObject
{
    private int _upgradeLvl;
    private LogicTimer _constructionTimer;
    private bool _upgrading;
    private bool _locked;

    public LogicBuilding(LogicBuildingData data, LogicLevel level) : base(data, level)
    {
        LogicBuildingData buildingData = this.GetBuildingData();

        this._locked = buildingData.IsLocked();

        Debugger.Print($"USC: {buildingData.GetUnitStorageCapacity(0)}");
        if (buildingData.GetUnitStorageCapacity(0) >= 1)
        {
            if (buildingData.IsAllianceCastle())
            {
                //TODO: add bunker comp.
            }
            else
            {
                this.AddComponent(new LogicUnitStorageComponent(this, 0));
            }
        }
    }

    public LogicBuildingData GetBuildingData()
    {
        return (LogicBuildingData)this._data;
    }

    public override int GetGameObjectType()
    {
        return 0;
    }

    public override int GetHeightInTiles()
    {
        return this.GetBuildingData().GetHeight();
    }

    public override int GetWidthInTiles()
    {
        return this.GetBuildingData().GetWidth();
    }

    public void StartConstructing()
    {
        int time = this.GetBuildingData().GetConstructionTime(this._upgradeLvl);

        if (time <= 0)
            this.FinishConstruction(false);
        else
        {
            this._constructionTimer = new LogicTimer();
            this._constructionTimer.StartTimer(time, this._level.GetLogicTime());

            this._level.GetWorkerManager().AllocateWorker(this);
        }
    }

    public void FinishConstruction(bool value)
    {
        int state = this._level.GetState();

        // TODO!!
    }

    public override void Save(LogicJSONObject jsonObject)
    {
        if (this._upgradeLvl >= 0 || this._constructionTimer != null || this._upgrading)
            jsonObject.Put("lvl", new LogicJSONNumber(this._upgradeLvl));
        else
            jsonObject.Put("lvl", new LogicJSONNumber(-1));

        if (this._constructionTimer != null)
            jsonObject.Put("const_t", new LogicJSONNumber(this._constructionTimer.GetRemainingSeconds(this._level.GetLogicTime())));

        if (this._locked)
            jsonObject.Put("locked", new LogicJSONBoolean(true));

        base.Save(jsonObject);
    }

    public override void Load(LogicJSONObject jsonObject)
    {
        LogicJSONNumber lvl = jsonObject.GetJSONNumber("lvl");

        if (lvl != null)
        {
            this._upgradeLvl = lvl.GetIntValue();
            int upgradeLevelCnt = this.GetBuildingData().GetUpgradeLevelCnt();

            if (this._upgradeLvl <= upgradeLevelCnt)
            {
                if (this._upgradeLvl < -1)
                {
                    Debugger.Error("LogicBuilding.Load - Loaded an illegal upgrade level!");
                }
            }
            else
            {
                Debugger.Warning($"LogicBuilding.Load - Loaded upgrade level {this._upgradeLvl} is over max! (max = {upgradeLevelCnt})");
                this._upgradeLvl = upgradeLevelCnt - 1;
            }

            LogicJSONBoolean lockedObject = jsonObject.GetJSONBoolean("locked");

            if (lockedObject != null)
            {
                this._locked = lockedObject.IsTrue();
            }
            else
            {
                this._locked = false;
            }
        }

        base.Load(jsonObject);
    }
}
