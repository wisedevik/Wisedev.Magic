﻿using System.Data;
using Wisedev.Magic.Titam.CSV;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Data;

public class LogicDataTables
{
    public const int TABLE_COUNT = 52;

    private static LogicDataTable[] _tables;

    public static void Init()
    {
        LogicDataTables._tables = new LogicDataTable[LogicDataTables.TABLE_COUNT];
    }

    public static void InitDataTable(CSVNode node, LogicDataType index)
    {
        if (LogicDataTables._tables[(int)index] != null)
        {
            LogicDataTables._tables[(int)index].SetTable(node.GetTable());
        }
        else
        {
            LogicDataTables._tables[(int)index] = new LogicDataTable(node.GetTable(), index);
        }
    }

    public static void CreateReferences()
    {
        for (int i = 0; i < LogicDataTables.TABLE_COUNT; i++)
        {
            if (LogicDataTables._tables[i] != null)
            {
                LogicDataTables._tables[i].CreateReferences();
            }
        }
    }

    public static LogicDataTable GetTable(LogicDataType tableIndex)
    {
        return LogicDataTables._tables[(int)tableIndex];
    }

    public static LogicData? GetDataById(int globalId)
    {
        int tableIndex = GlobalID.GetClassID(globalId) - 1;

        if (tableIndex >= 0 && tableIndex < LogicDataTables.TABLE_COUNT && LogicDataTables._tables[tableIndex] != null)
        {
            return LogicDataTables._tables[tableIndex].GetItemById(globalId);
        }

        return null;
    }

    public static LogicData GetDataById(int globalId, LogicDataType dataType)
    {
        LogicData data = LogicDataTables.GetDataById(globalId);

        if (data.GetDataType() != dataType)
            return null;

        return data;
    }

    public static LogicResourceData GetResourceByName(string name, LogicData? caller)
    {
        return (LogicResourceData)LogicDataTables._tables[(int)LogicDataType.RESOURCE].GetDataByName(name, caller);
    }

    public static LogicAllianceBadgeData GetBadgeByName(string name, LogicData? caller)
    {
        return (LogicAllianceBadgeData)LogicDataTables._tables[(int)LogicDataType.ALLIANCE_BADGE].GetDataByName(name, caller);
    }
}
