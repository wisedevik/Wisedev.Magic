using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Utils;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicTownhallLevelData : LogicData
{
    private List<int> _buildingCaps;

    public LogicTownhallLevelData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();

        _buildingCaps = new List<int>();

        LogicTownhallLevelData previousItem = null;

        if (GetInstanceID() > 0)
        {
            previousItem = (LogicTownhallLevelData)_table.GetItemAt(GetInstanceID() - 1);
        }

        LogicDataTable buildingTable = LogicDataTables.GetTable(LogicDataType.BUILDING);

        for (int i = 0; i < buildingTable.GetItemCount(); i++)
        {
            LogicData item = buildingTable.GetItemAt(i);

            int cap = _row.GetIntegerValue(item.GetName(), 0);
            int gearup = _row.GetIntegerValue(item.GetName() + "_gearup", 0);

            if (previousItem != null)
            {
                if (cap == 0)
                {
                    cap = previousItem._buildingCaps[i];
                }

            }

            _buildingCaps.Add(cap);
        }
    }

    public int GetUnlockedBuildingCount(LogicBuildingData data)
    {
        return _buildingCaps[data.GetInstanceID()];
    }
}
