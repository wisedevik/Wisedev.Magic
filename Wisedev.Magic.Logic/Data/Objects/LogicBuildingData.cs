using System;
using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Math;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicBuildingData : LogicData
{
    private LogicBuildingClassData _buildingClass;

    private int _upgradeLevelCount;

    private LogicResourceData[] _buildResourceData;

    private int _width;
    private int _height;
    private bool _locked;
    private bool _forgesSpells;
    private int[] _housingSpace;
    private bool _bunker;
    private int[] _constructionTimes;
    private int[] _townHallLevel;
    private int[] _buildCost;

    LogicHeroData _heroData;

    private bool _premiumCurrency;

    public LogicBuildingData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _upgradeLevelCount = _row.GetLongestArraySize();
        _buildingClass = LogicDataTables.GetBuildingClassByName(_row.GetValue("BuildingClass", 0), this);

        _width = _row.GetIntegerValue("Width", 0);
        _height = _row.GetIntegerValue("Height", 0);

        _bunker = _row.GetBooleanValue("Bunker", 0);

        _locked = _row.GetBooleanValue("Locked", 0);
        _forgesSpells = _row.GetBooleanValue("ForgesSpells", 0);

        int longestArraySize = _row.GetLongestArraySize();
        
        _buildCost = new int[longestArraySize];
        _housingSpace = new int[longestArraySize];
        _constructionTimes = new int[longestArraySize];
        _townHallLevel = new int[longestArraySize];
        _buildResourceData = new LogicResourceData[longestArraySize];

        for (int i = 0; i < longestArraySize; i++)
        {
            _buildCost[i] = _row.GetClampedIntegerValue("BuildCost", i);

            _housingSpace[i] = _row.GetClampedIntegerValue("HousingSpace", i);

            _constructionTimes[i] = 86400 * _row.GetClampedIntegerValue("BuildTimeD", i) +
                                              3600 * _row.GetClampedIntegerValue("BuildTimeH", i) +
                                              60 * _row.GetClampedIntegerValue("BuildTimeM", i) +
                                              _row.GetIntegerValue("BuildTimeS", i);
            _buildResourceData[i] = LogicDataTables.GetResourceByName(_row.GetClampedValue("BuildResource", i), this);
            _townHallLevel[i] = LogicMath.Max(_row.GetClampedIntegerValue("TownHallLevel", i) - 1, 0);
        }


        string heroType = _row.GetValue("HeroType", 0);

        if (!string.IsNullOrEmpty(heroType))
        {
            _heroData = LogicDataTables.GetHeroByName(heroType, this);
        }
    }

    public bool IsPremiumCurrency()
    {
        return _premiumCurrency;
    }

    public LogicBuildingClassData GetBuildingClass()
    {
        return _buildingClass;
    }

    public LogicResourceData GetBuildResource(int idx)
    {
        return _buildResourceData[idx];
    }

    public bool IsWorkerBuilding()
    {
        return _buildingClass.IsWorkerClass();
    }

    public int GetBuildCost(int index, LogicLevel level)
    {
        if (_buildingClass.IsWorkerClass())
        {
            return LogicDataTables.GetGlobals().GetWorkerCost(level);
        }
        else
        {
            return _buildCost[index];
        }

    }

    public int GetRequiredTownHallLevel(int idx)
    {
        if (idx != 0 || LogicDataTables.GetTownHallLevelCount() <= 0)
            return _townHallLevel[idx];

        for (int i = 0; i < LogicDataTables.GetTownHallLevelCount(); i++)
        {
            LogicTownhallLevelData townHallLevel = LogicDataTables.GetTownHallLevel(i);

            if (townHallLevel.GetUnlockedBuildingCount(this) > 0)
            {
                return i;
            }
        }

        return _townHallLevel[idx];
    }

    public bool IsTownHall()
    {
        return _buildingClass.IsTownHallClass();
    }

    public int GetUpgradeLevelCount()
    {
        return _upgradeLevelCount;
    }

    public LogicHeroData GetHeroData()
    {
        return _heroData;
    }

    public bool IsLocked()
    {
        return _locked;
    }

    public bool IsForgesSpells()
    {
        return _forgesSpells;
    }

    public int GetUnitStorageCapacity(int level)
    {
        
        return _housingSpace[level];
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    public bool IsAllianceCastle()
    {
        return _bunker;
    }

    public int GetConstructionTime(int upgLevel)
    {
        return _constructionTimes[upgLevel];
    }
}
