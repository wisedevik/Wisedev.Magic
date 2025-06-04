using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicProjectileData : LogicData
{
    private int _startHeight;
    private int _startOffset;
    private bool _randomHitPosition;
    private bool _isBallistic;
    private int _speed;
    private bool _useRotate;

    public LogicProjectileData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _startOffset = _row.GetIntegerValue("StartOffset", 0);
        _startHeight = _row.GetIntegerValue("StartHeight", 0);
        _speed = _row.GetIntegerValue("Speed", 0);
    }

    public int GetSpeed()
    {
        return _speed;
    }

    public int GetStartOffset()
    {
        return _startOffset;
    }
}
