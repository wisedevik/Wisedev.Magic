using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicObstacleData : LogicData
{
    private bool _isTombstone;

    public LogicObstacleData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _isTombstone = _row.GetBooleanValue("IsTombstone", 0);
    }

    public bool IsTombstone()
    {
        return _isTombstone;
    }
}
