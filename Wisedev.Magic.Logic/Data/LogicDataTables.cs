using System.Data;
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

    public static void InitDataTable(CSVNode node, int index)
    {
        if (LogicDataTables._tables[index] != null)
        {
            LogicDataTables._tables[index].SetTable(node.GetTable());
        }
        else
        {
            LogicDataTables._tables[index] = new LogicDataTable(node.GetTable(), index);
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

    public static LogicDataTable GetTable(int tableIndex)
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

    public static LogicData GetDataById(int globalId, int dataType)
    {
        LogicData data = LogicDataTables.GetDataById(globalId);

        if (data.GetDataType() != dataType)
            return null;

        return data;
    }

    public static LogicResourceData GetResourceByName(string name, LogicData? caller)
    {
        return (LogicResourceData)LogicDataTables._tables[2].GetDataByName(name, caller);
    }
}
