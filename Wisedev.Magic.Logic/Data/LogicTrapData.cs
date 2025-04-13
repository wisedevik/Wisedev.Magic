using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data;

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
        this._width = this._row.GetIntegerValue("Width", 0);
        this._height = this._row.GetIntegerValue("Height", 0);
    }

    public int GetWidth()
    {
        return this._width;
    }

    public int GetHeight()
    {
        return this._height;
    }
}
