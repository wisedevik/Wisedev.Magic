using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicLeagueData : LogicData
{
    private string _iconSWF;
    private int _demoteLimit;
    private int _promoteLimit;
    private int _placementLimitLow;
    private int _placementLimitHigh;

    public LogicLeagueData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _iconSWF = _row.GetValue("IconSWF", 0);
        _demoteLimit = _row.GetIntegerValue("DemoteLimit", 0);
        _promoteLimit = _row.GetIntegerValue("PromoteLimit", 0);
        _placementLimitLow = _row.GetIntegerValue("PlacementLimitLow", 0);
        _placementLimitHigh = _row.GetIntegerValue("PlacementLimitHigh", 0);
    }
}
