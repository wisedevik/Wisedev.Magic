using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicTrapData : LogicData
{
    private int _width;
    private int _height;

    public LogicTrapData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _width = _row.GetIntegerValue("Width", 0);
        _height = _row.GetIntegerValue("Height", 0);
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }
}
