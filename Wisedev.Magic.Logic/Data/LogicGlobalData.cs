using Wisedev.Magic.Titam.CSV;

namespace Wisedev.Magic.Logic.Data;

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
        int size = this._row.GetLongestArraySize();

        this._numberArray = new int[size];

        this._numberValue = this._row.GetIntegerValue("NumberValue", 0);
        this._booleanValue = this._row.GetBooleanValue("BooleanValue", 0);
        this._textValue = this._row.GetValue("TextValue", 0);

        for (int i = 0; i < size; i++)
        {
            this._numberArray[i] = this._row.GetIntegerValue("NumberValue", 0);
        }
    }

    public int GetNumberValue()
    {
        return this._numberValue;
    }

    public bool GetBooleanValue()
    {
        return this._booleanValue;
    }

    public string GetTextValue()
    {
        return this._textValue;
    }

    public int GetNumberArray(int index)
    {
        return this._numberArray[index];
    }
}
