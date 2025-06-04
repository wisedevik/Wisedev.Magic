using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

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
        _timeH = _row.GetIntegerValue("TimeH", 0);
        _diamonds = _row.GetIntegerValue("Diamonds", 0);
        _lockedAboveScore = _row.GetIntegerValue("LockedAboveScore", 0);
    }

    public int GetTimeH()
    {
        return _timeH;
    }

    public int GetDiamonds()
    {
        return _diamonds;
    }

    public int IsLockedAboveScore()
    {
        return _lockedAboveScore;
    }
}
