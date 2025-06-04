using Wisedev.Magic.Logic.Data.Core;

namespace Wisedev.Magic.Logic.Data.Tables;

public class LogicDataTableResource
{
    private string _fileName;

    private LogicDataType _tableIndex;
    private int _type;

    public LogicDataTableResource(string fileName, LogicDataType tableIndex, int type)
    {
        _fileName = fileName;
        _tableIndex = tableIndex;
        _type = type;
    }

    public void Destruct()
    {
        _fileName = null;
        _tableIndex = 0;
        _type = 0;
    }

    public string GetFileName()
    {
        return _fileName;
    }

    public LogicDataType GetTableIndex()
    {
        return _tableIndex;
    }

    public int GetTableType()
    {
        return _type;
    }
}
