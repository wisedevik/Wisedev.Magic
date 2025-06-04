using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicExperienceLevelData : LogicData
{
    public LogicExperienceLevelData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        LogicDataTable table = LogicDataTables.GetTable(LogicDataType.EXPERIENCE_LEVEL);
        if (table.GetItemCount() >= 1)
        {
            for (int i = 0; i < table.GetItemCount(); i++)
            {
               
            }
        }
    }

    public static int GetLevelCap()
    {
        return LogicDataTables.GetTable(LogicDataType.EXPERIENCE_LEVEL).GetItemCount();
    }
}
