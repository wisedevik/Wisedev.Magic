using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data;

public class LogicDecoData : LogicData
{
    private int _width;
    private int _height;
    private int _buildCost;
    private LogicResourceData _buildResourceData;

    public LogicDecoData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        this._width = this._row.GetIntegerValue("Width", 0);
        this._height = this._row.GetIntegerValue("Height", 0);
        this._buildCost = this._row.GetIntegerValue("BuildCost", 0);

        this._buildResourceData = LogicDataTables.GetResourceByName(this._row.GetValue("BuildResource", 0), this);
    }

    public int GetWidth()
    {
        return this._width;
    }

    public int GetHeight()
    {
        return this._height;
    }

    public int GetBuildCost()
    {
        return this._buildCost;
    }

    public LogicResourceData GetBuildResource()
    {
        return this._buildResourceData;
    }
}
