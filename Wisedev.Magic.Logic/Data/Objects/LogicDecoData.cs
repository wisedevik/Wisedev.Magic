using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

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
        _width = _row.GetIntegerValue("Width", 0);
        _height = _row.GetIntegerValue("Height", 0);
        _buildCost = _row.GetIntegerValue("BuildCost", 0);

        _buildResourceData = LogicDataTables.GetResourceByName(_row.GetValue("BuildResource", 0), this);
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    public int GetBuildCost()
    {
        return _buildCost;
    }

    public LogicResourceData GetBuildResource()
    {
        return _buildResourceData;
    }
}
