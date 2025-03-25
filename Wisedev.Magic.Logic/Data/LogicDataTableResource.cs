namespace Wisedev.Magic.Logic.Data;

public class LogicDataTableResource
{
    private string _fileName;

    private LogicDataType _tableIndex;
    private int _type;

    public LogicDataTableResource(string fileName, LogicDataType tableIndex, int type)
    {
        this._fileName = fileName;
        this._tableIndex = tableIndex;
        this._type = type;
    }

    public void Destruct()
    {
        this._fileName = null;
        this._tableIndex = 0;
        this._type = 0;
    }

    public string GetFileName()
    {
        return this._fileName;
    }

    public LogicDataType GetTableIndex()
    {
        return this._tableIndex;
    }

    public int GetTableType()
    {
        return this._type;
    }
}
