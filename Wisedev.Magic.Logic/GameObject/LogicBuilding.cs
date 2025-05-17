using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject.Component;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Debug;
using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Util;
using Wisedev.Magic.Titan.Math;

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

        if (buildingData.GetHeroData() != null)
        {
            LogicHeroBaseComponent logicHeroBaseComponent = new LogicHeroBaseComponent(this, buildingData.GetHeroData());
            this.AddComponent(logicHeroBaseComponent);
        }

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

    public void AdjustConstructionTime(int secondsPassed)
    {
        if (_constructionTimer != null)
        {
            int remaining = _constructionTimer.GetRemainingSeconds(_level.GetLogicTime());
            remaining = Math.Max(remaining - secondsPassed, 0);

            if (remaining <= 0)
            {
                FinishConstruction(false);
            }
            else
            {
                _constructionTimer.StartTimer(remaining, _level.GetLogicTime());
            }
        }
    }

    public override void Tick()
    {
        base.Tick();

        if (_constructionTimer != null)
        {
            if (this._constructionTimer.GetRemainingSeconds(this._level.GetLogicTime()) <= 0)
                this.FinishConstruction(false);
        }

    }

    public void StartUpgrading()
    {

        if (_constructionTimer != null)
        {
            _constructionTimer.Destruct();
            _constructionTimer = null;
        }

        _upgrading = true;

        int time = this.GetBuildingData().GetConstructionTime(this._upgradeLvl + 1);
        if (time < 1)
            this.FinishConstruction(false);
        else
        {
            _level.GetWorkerManager().AllocateWorker(this);

            //TODO: off components;

            _constructionTimer = new LogicTimer();
            _constructionTimer.StartTimer(time, _level.GetLogicTime());

            Debugger.Print($"Update started ({this.GetBuildingData().GetName()}) {_constructionTimer.GetRemainingSeconds(_level.GetLogicTime())}s. left");
        }

    }

    public bool CanUpgrade(bool value)
    {
        if (_constructionTimer == null)
        {
            int upgradeLevelCount = this.GetBuildingData().GetUpgradeLevelCount() - 1;
            if (this._upgradeLvl < upgradeLevelCount)
            {
                if (!GetBuildingData().GetBuildingClass().IsTownHallClass())
                {
                    int townHallLevel = _level.GetTownHallLevel();
                    int requiredLvl = GetBuildingData().GetRequiredTownHallLevel(Math.Min(this._upgradeLvl + 1, GetBuildingData().GetUpgradeLevelCount() - 1));
                    if (townHallLevel < requiredLvl)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        return false;
    }

    public int GetRequiredTownHallLevelForUpgrade()
    {
        return this.GetBuildingData().GetRequiredTownHallLevel(LogicMath.Min(this._upgradeLvl + 1, this.GetBuildingData().GetUpgradeLevelCount() - 1));
    }

    public int GetUpgradeLevel()
    {
        return this._upgradeLvl;
    }

    public bool SpeedUpConstruction()
    {
        if (_constructionTimer != null)
        {
            LogicClientAvatar playerAvatar = this._level.GetPlayerAvatar();
            int speedUpCost = LogicGamePlayUtil.GetSpeedUpCost(this._constructionTimer.GetRemainingSeconds(this._level.GetLogicTime()), 0);

            if (playerAvatar.HasEnoughDiamonds(speedUpCost, true, this._level))
            {
                playerAvatar.UseDiamonds(speedUpCost);
                this.FinishConstruction(false);
                return true;
            }
        }

        return false;
    }

    public LogicHeroBaseComponent GetHeroBaseComponent()
    {
        return (LogicHeroBaseComponent)this.GetComponent(10);
    }

    public LogicBuildingData GetBuildingData()
    {
        return (LogicBuildingData)this._data;
    }

    public override int GetGameObjectType()
    {
        return 0;
    }

    public int GetRemainingConstructionTime()
    {
        return _constructionTimer.GetRemainingSeconds(this._level.GetLogicTime());
    }

    public bool IsConstructing()
    {
        return _constructionTimer != null;
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
        if (this._constructionTimer != null)
        {
            _constructionTimer.Destruct();
            _constructionTimer = null;
        }

        int time = this.GetBuildingData().GetConstructionTime(this._upgradeLvl);

        if (time < 1)
            this.FinishConstruction(false);
        else
        {
            this._constructionTimer = new LogicTimer();
            this._constructionTimer.StartTimer(time, this._level.GetLogicTime());

            this._level.GetWorkerManager().AllocateWorker(this);
        }
    }

    public void SetUpgradeLevel(int level)
    {
        LogicBuildingData buildingData = (LogicBuildingData)this._data;

        this._upgradeLvl = LogicMath.Clamp(level, 0, buildingData.GetUpgradeLevelCount() - 1);
        if (this._level.GetHomeOwnerAvatar() != null)
        {
            if (buildingData.IsAllianceCastle())
            {

            }
            else if (buildingData.IsTownHall())
            {
                _level.GetHomeOwnerAvatar().SetTownHallLevel(this._upgradeLvl);
            }
        }
    }

    public void FinishConstruction(bool value)
    {
        int state = this._level.GetState();
        if (state == 1 || !LogicDataTables.GetGlobals().CompleteConstructionsOnlyHome() && value)
        {
            if (this._level.GetHomeOwnerAvatar() != null)
            {
                if (this._level.GetHomeOwnerAvatar().IsClientAvatar())
                {
                    if (this._constructionTimer != null)
                    {
                        this._constructionTimer.Destruct();
                        this._constructionTimer = null;
                    }

                    _level.GetWorkerManager().DeallocateWorker(this);
                    Debugger.Print($"Finished for {GetBuildingData().GetName()}");

                    if (_upgradeLvl != 0 || _upgrading)
                    {
                        int newLvl = _upgradeLvl + 1;

                        Debugger.Print($"New level for {GetBuildingData().GetName()} = {newLvl}");
                        this.SetUpgradeLevel(newLvl);
                    }

                    _upgrading = false;
                }
            }
        }
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
            int upgradeLevelCnt = this.GetBuildingData().GetUpgradeLevelCount();

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

        LogicJSONNumber constTimeObject = jsonObject.GetJSONNumber("const_t");
        if (this._constructionTimer != null)
        {
            this._constructionTimer.Destruct();
            this._constructionTimer = null;
        }

        if (constTimeObject != null)
        {
            int secs = constTimeObject.GetIntValue();
            secs = LogicMath.Min(secs, this.GetBuildingData().GetConstructionTime(this._upgradeLvl + 1));

            this._constructionTimer = new LogicTimer();
            this._constructionTimer.StartTimer(secs, this._level.GetLogicTime());

            Debugger.Print($"Remaining seconds loaded {_constructionTimer.GetRemainingSeconds(_level.GetLogicTime())}s. for {this.GetBuildingData().GetName()}");

            _level.GetWorkerManager().AllocateWorker(this);
            this._upgrading = this._upgradeLvl != -1;
        }

        base.Load(jsonObject);
    }

    public override int GetType()
    {
        return 0;
    }
}
