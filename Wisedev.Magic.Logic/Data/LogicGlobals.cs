using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Debug;
using Wisedev.Magic.Titan.Math;

namespace Wisedev.Magic.Logic.Data;

public class LogicGlobals : LogicDataTable
{
    private int _startingDiamonds;
    private int _startingGold;
    private int _startingElixir;
    private bool _removeRevengeWhenBattleIsLoaded;
    private int _newbieProtectionLevel;
    private int _spellSpeedUpCostMultiplier;
    private int _troopRequestSpeedUpCostMultiplier;
    private int _heroHealthSpeedUpCostMultiplier;
    private bool _useNewSpeedUpCalculation;
    private int _speedUpDiamondCostPerMin;
    private int _speedUpDiamondCostPerHour;
    private int _speedUpDiamondCostPerDay;
    private int _speedUpDiamondCostPerWeek;
    private bool _completeConstructionsOnlyHome;
    private int _attackLengthSec;
    private int _workerCost2nd;
    private int _workerCost3rd;
    private int _workerCost4th;
    private int _workerCost5th;
    private bool _workerForZeroBuildingTime;

    public LogicGlobals(CSVTable table, LogicDataType tableIdx) : base(table, tableIdx)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        this._startingDiamonds = this.GetGlobalData("STARTING_DIAMONDS").GetNumberValue();
        this._startingGold = this.GetGlobalData("STARTING_GOLD").GetNumberValue();
        this._startingElixir = this.GetGlobalData("STARTING_ELIXIR").GetNumberValue();
        this._removeRevengeWhenBattleIsLoaded = this.GetGlobalData("REMOVE_REVENGE_WHEN_BATTLE_IS_LOADED").GetBooleanValue();
        this._newbieProtectionLevel = this.GetGlobalData("NEWBIE_PROTECTION_LEVEL").GetNumberValue() - 1;
        this._spellSpeedUpCostMultiplier = this.GetGlobalData("SPELL_SPEED_UP_COST_MULTIPLIER").GetNumberValue();
        this._troopRequestSpeedUpCostMultiplier = this.GetGlobalData("TROOP_REQUEST_SPEED_UP_COST_MULTIPLIER").GetNumberValue();
        this._heroHealthSpeedUpCostMultiplier = this.GetGlobalData("HERO_HEALTH_SPEED_UP_COST_MULTIPLIER").GetNumberValue();
        this._useNewSpeedUpCalculation = this.GetGlobalData("USE_NEW_SPEEDUP_CALCULATION").GetBooleanValue();
        this._speedUpDiamondCostPerMin = this.GetGlobalData("SPEED_UP_DIAMOND_COST_1_MIN").GetNumberValue();
        this._speedUpDiamondCostPerHour = this.GetGlobalData("SPEED_UP_DIAMOND_COST_1_HOUR").GetNumberValue();
        this._speedUpDiamondCostPerDay = this.GetGlobalData("SPEED_UP_DIAMOND_COST_24_HOURS").GetNumberValue();
        this._speedUpDiamondCostPerWeek = this.GetGlobalData("SPEED_UP_DIAMOND_COST_1_WEEK").GetNumberValue();
        this._completeConstructionsOnlyHome = this.GetGlobalData("COMPLETE_CONSTRUCTIONS_ONLY_HOME").GetBooleanValue();
        this._attackLengthSec = this.GetGlobalData("ATTACK_LENGTH_SEC").GetNumberValue();
        this._workerCost2nd = this.GetGlobalData("WORKER_COST_2ND").GetNumberValue();
        this._workerCost3rd = this.GetGlobalData("WORKER_COST_3RD").GetNumberValue();
        this._workerCost4th = this.GetGlobalData("WORKER_COST_4TH").GetNumberValue();
        this._workerCost5th = this.GetGlobalData("WORKER_COST_5TH").GetNumberValue();
        this._workerForZeroBuildingTime = this.GetGlobalData("WORKER_FOR_ZERO_BUILD_TIME").GetBooleanValue();
    }

    public LogicGlobalData? GetGlobalData(string name)
    {
        LogicGlobalData data = (LogicGlobalData)this.GetDataByName(name, null);
        if (data == null)
        {
            Debugger.Error($"Unable to find global: {name}");
            return null;
        }

        return data;
    }

    public bool WorkerForZeroBuildTime()
    {
        return this._workerForZeroBuildingTime;
    }

    public int GetWorkerCost(LogicLevel level)
    {
        int totalWorkers = level.GetWorkerManager().GetTotalWorkers();
        if (totalWorkers <= 1)
            return _workerCost2nd;
        else if (totalWorkers == 2)
            return _workerCost3rd;
        else if (totalWorkers == 3)
            return _workerCost4th;
        else
            return _workerCost5th;
    }

    public int GetAttackLengthSec()
    {
        return this._attackLengthSec;
    }

    public bool CompleteConstructionsOnlyHome()
    {
        return this._completeConstructionsOnlyHome;
    }

    public int GetSpeedUpCost(int time, int multiplier)
    {
        if (time >= 1)
        {
            int speedUpDiamondCostPerMin;
            int speedUpDiamondCostPerHour;
            int speedUpDiamondCostPerDay;
            int speedUpDiamondCostPerWeek;

            int multiplier1 = multiplier;
            int multiplier2 = 100;

            speedUpDiamondCostPerMin = this._speedUpDiamondCostPerMin;
            speedUpDiamondCostPerHour = this._speedUpDiamondCostPerHour;
            speedUpDiamondCostPerDay = this._speedUpDiamondCostPerDay;
            speedUpDiamondCostPerWeek = this._speedUpDiamondCostPerWeek;

            if (this._useNewSpeedUpCalculation)
            {
                multiplier1 = 100;
                multiplier2 = multiplier;
            }

            int cost = speedUpDiamondCostPerMin;

            if (time >= 60)
            {
                if (time >= 3600)
                {
                    if (time >= 86400)
                    {
                        int tmp1 = (speedUpDiamondCostPerWeek - speedUpDiamondCostPerDay) * (time - 86400);

                        cost = multiplier2 * speedUpDiamondCostPerDay / 100 + tmp1 / 100 * multiplier2 / 518400;

                        if (cost < 0 || tmp1 / 100 > 0x7FFFFFFF / multiplier2)
                        {
                            cost = multiplier2 * (speedUpDiamondCostPerDay + tmp1 / 518400) / 100;
                        }
                    }
                    else
                    {
                        cost = multiplier2 * speedUpDiamondCostPerHour / 100 +
                               (speedUpDiamondCostPerDay - speedUpDiamondCostPerHour) * (time - 3600) / 100 * multiplier2 / 82800;
                    }
                }
                else
                {
                    cost = multiplier2 * speedUpDiamondCostPerMin / 100 + (speedUpDiamondCostPerHour - speedUpDiamondCostPerMin) * (time - 60) * multiplier2 / 354000;
                }
            }
            else if (this._useNewSpeedUpCalculation)
            {
                cost = multiplier2 * speedUpDiamondCostPerMin * time / 6000;
            }

            return LogicMath.Max(cost * multiplier1 / 100, 1);
        }

        return 0;
    }

    public int GetHeroHealthSpeedUpCostMultiplier()
    {
        return this._heroHealthSpeedUpCostMultiplier;
    }

    public int GetTroopRequestSpeedUpCostMultiplier()
    {
        return this._troopRequestSpeedUpCostMultiplier;
    }

    public int GetSpellSpeedUpCostMultiplier()
    {
        return this._spellSpeedUpCostMultiplier;
    }

    public int GetStartingDiamonds()
    {
        return this._startingDiamonds;
    }

    public int GetStartingGold()
    {
        return this._startingGold;
    }

    public int GetStartingElxir()
    {
        return this._startingElixir;
    }

    public bool RemoveRevengeWhenBattleIsLoaded()
    {
        return this._removeRevengeWhenBattleIsLoaded;
    }

    public int GetNewbieProtectionLevel()
    {
        return this._newbieProtectionLevel;
    }
}
