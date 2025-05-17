using System;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Math;

namespace Wisedev.Magic.Logic.Data;

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
        this._upgradeLevelCount = this._row.GetLongestArraySize();
        _buildingClass = LogicDataTables.GetBuildingClassByName(this._row.GetValue("BuildingClass", 0), this);

        this._width = this._row.GetIntegerValue("Width", 0);
        this._height = this._row.GetIntegerValue("Height", 0);

        this._bunker = this._row.GetBooleanValue("Bunker", 0);

        this._locked = this._row.GetBooleanValue("Locked", 0);
        this._forgesSpells = this._row.GetBooleanValue("ForgesSpells", 0);

        int longestArraySize = this._row.GetLongestArraySize();
        
        _buildCost = new int[longestArraySize];
        this._housingSpace = new int[longestArraySize];
        this._constructionTimes = new int[longestArraySize];
        _townHallLevel = new int[longestArraySize];
        _buildResourceData = new LogicResourceData[longestArraySize];

        for (int i = 0; i < longestArraySize; i++)
        {
            _buildCost[i] = this._row.GetClampedIntegerValue("BuildCost", i);

            int raw = _row.GetClampedIntegerValue("HousingSpace", i);
            _housingSpace[i] = (raw == int.MaxValue ? 0 : raw);

            int rawBuildTimeD = this._row.GetClampedIntegerValue("BuildTimeD", i);
            int rawBuildTimeH = this._row.GetClampedIntegerValue("BuildTimeH", i);
            int rawBuildTimeM = this._row.GetClampedIntegerValue("BuildTimeM", i);
            int rawBuildTimeS = this._row.GetIntegerValue("BuildTimeS", i);

            this._constructionTimes[i] = 86400 * (rawBuildTimeD == int.MaxValue ? 0 : rawBuildTimeD) +
                                              3600 * (rawBuildTimeH == int.MaxValue ? 0 : rawBuildTimeH) +
                                              60 * (rawBuildTimeM == int.MaxValue ? 0 : rawBuildTimeM) +
                                              (rawBuildTimeS == int.MaxValue ? 0 : rawBuildTimeS);
            _buildResourceData[i] = LogicDataTables.GetResourceByName(this._row.GetClampedValue("BuildResource", i), this);
            _townHallLevel[i] = LogicMath.Max(this._row.GetClampedIntegerValue("TownHallLevel", i) - 1, 0);
        }


        string heroType = this._row.GetValue("HeroType", 0);

        if (!string.IsNullOrEmpty(heroType))
        {
            this._heroData = LogicDataTables.GetHeroByName(heroType, this);
        }
    }

    public bool IsPremiumCurrency()
    {
        return this._premiumCurrency;
    }

    public LogicBuildingClassData GetBuildingClass()
    {
        return this._buildingClass;
    }

    public LogicResourceData GetBuildResource(int idx)
    {
        return this._buildResourceData[idx];
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
        return this._heroData;
    }

    public bool IsLocked()
    {
        return this._locked;
    }

    public bool IsForgesSpells()
    {
        return this._forgesSpells;
    }

    public int GetUnitStorageCapacity(int level)
    {
        
        return this._housingSpace[level];
    }

    public int GetWidth()
    {
        return this._width;
    }

    public int GetHeight()
    {
        return this._height;
    }

    public bool IsAllianceCastle()
    {
        return this._bunker;
    }

    public int GetConstructionTime(int upgLevel)
    {
        return this._constructionTimes[upgLevel];
    }
}
