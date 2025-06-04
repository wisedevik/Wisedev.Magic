using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicGlobalData : LogicData
{
    private int _numberValue;
    private bool _booleanValue;
    private string _textValue;

    private int[] _numberArray;

    public LogicGlobalData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        int size = _row.GetLongestArraySize();

        _numberArray = new int[size];

        _numberValue = _row.GetIntegerValue("NumberValue", 0);
        _booleanValue = _row.GetBooleanValue("BooleanValue", 0);
        _textValue = _row.GetValue("TextValue", 0);

        for (int i = 0; i < size; i++)
        {
            _numberArray[i] = _row.GetIntegerValue("NumberValue", 0);
        }
    }

    public int GetNumberValue()
    {
        return _numberValue;
    }

    public bool GetBooleanValue()
    {
        return _booleanValue;
    }

    public string GetTextValue()
    {
        return _textValue;
    }

    public int GetNumberArray(int index)
    {
        return _numberArray[index];
    }
}
