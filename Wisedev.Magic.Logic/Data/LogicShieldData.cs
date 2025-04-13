using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data;

public class LogicShieldData : LogicData
{
    private int _timeH;
    private int _diamonds;
    private int _lockedAboveScore;

    public LogicShieldData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        this._timeH = this._row.GetIntegerValue("TimeH", 0);
        this._diamonds = this._row.GetIntegerValue("Diamonds", 0);
        this._lockedAboveScore = this._row.GetIntegerValue("LockedAboveScore", 0);
    }

    public int GetTimeH()
    {
        return this._timeH;
    }

    public int GetDiamonds()
    {
        return this._diamonds;
    }

    public int IsLockedAboveScore()
    {
        return this._lockedAboveScore;
    }
}
