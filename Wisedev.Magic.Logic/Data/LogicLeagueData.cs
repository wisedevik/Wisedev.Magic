using Wisedev.Magic.Titam.CSV;

namespace Wisedev.Magic.Logic.Data;

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
        this._iconSWF = this._row.GetValue("IconSWF", 0);
        this._demoteLimit = this._row.GetIntegerValue("DemoteLimit", 0);
        this._promoteLimit = this._row.GetIntegerValue("PromoteLimit", 0);
        this._placementLimitLow = this._row.GetIntegerValue("PlacementLimitLow", 0);
        this._placementLimitHigh = this._row.GetIntegerValue("PlacementLimitHigh", 0);
    }
}
