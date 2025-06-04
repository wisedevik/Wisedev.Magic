using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Debug;
using Wisedev.Magic.Titan.Math;

namespace Wisedev.Magic.Logic.Data.Objects;

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
    private int _heroHealMultiplier;

    public LogicGlobals(CSVTable table, LogicDataType tableIdx) : base(table, tableIdx)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _startingDiamonds = GetGlobalData("STARTING_DIAMONDS").GetNumberValue();
        _startingGold = GetGlobalData("STARTING_GOLD").GetNumberValue();
        _startingElixir = GetGlobalData("STARTING_ELIXIR").GetNumberValue();
        _removeRevengeWhenBattleIsLoaded = GetGlobalData("REMOVE_REVENGE_WHEN_BATTLE_IS_LOADED").GetBooleanValue();
        _newbieProtectionLevel = GetGlobalData("NEWBIE_PROTECTION_LEVEL").GetNumberValue() - 1;
        _spellSpeedUpCostMultiplier = GetGlobalData("SPELL_SPEED_UP_COST_MULTIPLIER").GetNumberValue();
        _troopRequestSpeedUpCostMultiplier = GetGlobalData("TROOP_REQUEST_SPEED_UP_COST_MULTIPLIER").GetNumberValue();
        _heroHealthSpeedUpCostMultiplier = GetGlobalData("HERO_HEALTH_SPEED_UP_COST_MULTIPLIER").GetNumberValue();
        _useNewSpeedUpCalculation = GetGlobalData("USE_NEW_SPEEDUP_CALCULATION").GetBooleanValue();
        _speedUpDiamondCostPerMin = GetGlobalData("SPEED_UP_DIAMOND_COST_1_MIN").GetNumberValue();
        _speedUpDiamondCostPerHour = GetGlobalData("SPEED_UP_DIAMOND_COST_1_HOUR").GetNumberValue();
        _speedUpDiamondCostPerDay = GetGlobalData("SPEED_UP_DIAMOND_COST_24_HOURS").GetNumberValue();
        _speedUpDiamondCostPerWeek = GetGlobalData("SPEED_UP_DIAMOND_COST_1_WEEK").GetNumberValue();
        _completeConstructionsOnlyHome = GetGlobalData("COMPLETE_CONSTRUCTIONS_ONLY_HOME").GetBooleanValue();
        _attackLengthSec = GetGlobalData("ATTACK_LENGTH_SEC").GetNumberValue();
        _workerCost2nd = GetGlobalData("WORKER_COST_2ND").GetNumberValue();
        _workerCost3rd = GetGlobalData("WORKER_COST_3RD").GetNumberValue();
        _workerCost4th = GetGlobalData("WORKER_COST_4TH").GetNumberValue();
        _workerCost5th = GetGlobalData("WORKER_COST_5TH").GetNumberValue();
        _workerForZeroBuildingTime = GetGlobalData("WORKER_FOR_ZERO_BUILD_TIME").GetBooleanValue();
        _heroHealMultiplier = GetGlobalData("HERO_HEAL_MULTIPLIER").GetNumberValue();
    }

    public LogicGlobalData? GetGlobalData(string name)
    {
        LogicGlobalData data = (LogicGlobalData)GetDataByName(name, null);
        if (data == null)
        {
            Debugger.Error($"Unable to find global: {name}");
            return null;
        }

        return data;
    }

    public int GetHeroHealMultiplier()
    {
        return _heroHealMultiplier;
    }

    public bool WorkerForZeroBuildTime()
    {
        return _workerForZeroBuildingTime;
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
        return _attackLengthSec;
    }

    public bool CompleteConstructionsOnlyHome()
    {
        return _completeConstructionsOnlyHome;
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

            speedUpDiamondCostPerMin = _speedUpDiamondCostPerMin;
            speedUpDiamondCostPerHour = _speedUpDiamondCostPerHour;
            speedUpDiamondCostPerDay = _speedUpDiamondCostPerDay;
            speedUpDiamondCostPerWeek = _speedUpDiamondCostPerWeek;

            if (_useNewSpeedUpCalculation)
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
            else if (_useNewSpeedUpCalculation)
            {
                cost = multiplier2 * speedUpDiamondCostPerMin * time / 6000;
            }

            return LogicMath.Max(cost * multiplier1 / 100, 1);
        }

        return 0;
    }

    public int GetHeroHealthSpeedUpCostMultiplier()
    {
        return _heroHealthSpeedUpCostMultiplier;
    }

    public int GetTroopRequestSpeedUpCostMultiplier()
    {
        return _troopRequestSpeedUpCostMultiplier;
    }

    public int GetSpellSpeedUpCostMultiplier()
    {
        return _spellSpeedUpCostMultiplier;
    }

    public int GetStartingDiamonds()
    {
        return _startingDiamonds;
    }

    public int GetStartingGold()
    {
        return _startingGold;
    }

    public int GetStartingElxir()
    {
        return _startingElixir;
    }

    public bool RemoveRevengeWhenBattleIsLoaded()
    {
        return _removeRevengeWhenBattleIsLoaded;
    }

    public int GetNewbieProtectionLevel()
    {
        return _newbieProtectionLevel;
    }
}
