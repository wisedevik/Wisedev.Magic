using System.Data;
using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Objects;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Data.Tables;

public class LogicDataTables
{
    public const int TABLE_COUNT = 52;

    private static LogicDataTable[] _tables;
    
    private static LogicResourceData _goldData;
    private static LogicResourceData _elixirData;

    public static void Init()
    {
        _tables = new LogicDataTable[TABLE_COUNT];
    }

    public static void InitDataTable(CSVNode node, LogicDataType index)
    {
        if (_tables[(int)index] != null)
        {
            _tables[(int)index].SetTable(node.GetTable());
        }
        else
        {
            switch (index)
            {
                case LogicDataType.GLOBAL:
                    _tables[(int)index] = new LogicGlobals(node.GetTable(), index);
                    break;
                default:
                    _tables[(int)index] = new LogicDataTable(node.GetTable(), index);
                    break;
            }
        }
    }

    public static void CreateReferences()
    {
        for (int i = 0; i < TABLE_COUNT; i++)
            if (_tables[i] != null)
                _tables[i].CreateReferences();

        _goldData = GetResourceByName("Gold", null);
        _elixirData = GetResourceByName("Elixir", null);
    }

    public static LogicDataTable GetTable(LogicDataType tableIndex)
    {
        return _tables[(int)tableIndex];
    }

    public static LogicData? GetDataById(int globalId)
    {
        int tableIndex = GlobalID.GetClassID(globalId) - 1;

        if (tableIndex >= 0 && tableIndex < TABLE_COUNT && _tables[tableIndex] != null)
        {
            return _tables[tableIndex].GetItemById(globalId);
        }

        return null;
    }

    public static LogicData GetDataById(int globalId, LogicDataType dataType)
    {
        LogicData data = GetDataById(globalId);

        if (data.GetDataType() != dataType)
            return null;

        return data;
    }

    public static LogicData GetDataByName(string name, LogicDataType dataType, LogicData? caller)
    {
        return _tables[(int)dataType].GetDataByName(name, caller);
    }

    public static LogicBuildingClassData GetBuildingClassByName(string name, LogicData? caller)
    {
        return (LogicBuildingClassData)_tables[(int)LogicDataType.BUILDING_CLASS].GetDataByName(name, caller);
    }

    public static LogicResourceData GetResourceByName(string name, LogicData? caller)
    {
        return (LogicResourceData)_tables[(int)LogicDataType.RESOURCE].GetDataByName(name, caller);
    }

    public static LogicHeroData GetHeroByName(string name, LogicData? caller)
    {
        return (LogicHeroData)_tables[(int)LogicDataType.HERO].GetDataByName(name, caller);
    }

    public static LogicAllianceBadgeData GetBadgeByName(string name, LogicData? caller)
    {
        return (LogicAllianceBadgeData)_tables[(int)LogicDataType.ALLIANCE_BADGE].GetDataByName(name, caller);
    }

    public static LogicGlobals GetGlobals()
    {
        return (LogicGlobals)_tables[(int)LogicDataType.GLOBAL];
    }

    public static LogicResourceData GetGoldData()
    {
        return _goldData;
    }

    public static LogicResourceData GetElixirData()
    {
        return _elixirData;
    }

    public static LogicCharacterData GetCharacterByName(string name, LogicData? caller)
    {
        return (LogicCharacterData)_tables[(int)LogicDataType.CHARACTER].GetDataByName(name, caller);
    }

    public static int GetTownHallLevelCount()
    {
        return _tables[(int)LogicDataType.TOWN_HALL_LEVEL].GetItemCount();
    }

    public static LogicTownhallLevelData GetTownHallLevel(int levelIndex)
    {
        if (levelIndex > -1)
        {
            if (levelIndex < _tables[(int)LogicDataType.TOWN_HALL_LEVEL].GetItemCount())
            {
                return (LogicTownhallLevelData)_tables[(int)LogicDataType.TOWN_HALL_LEVEL].GetItemAt(levelIndex);
            }
        }

        Debugger.Error("LogicDataTables::getTownHallLevel parameter out of bounds");

        return null;
    }
}
