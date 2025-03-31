using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Titam.DataStream;
using Wisedev.Magic.Titam.JSON;

namespace Wisedev.Magic.Logic.Util;

public class LogicDataSlot
{
    private LogicData _data;
    private int _count;

    public LogicData Data { get { return _data; } set { this._data = value; } }
    public int Count { get { return _count; } set { this._count = value; } }

    public LogicDataSlot(LogicData data, int count)
    {
        this._data = data;
        this._count = count;
    }

    public void Destruct()
    {
        this._data = null;
        this._count = 0;
    }

    public LogicDataSlot Clone()
    {
        return new LogicDataSlot(this._data, this._count);
    }

    public void Encode(ChecksumEncoder encoder)
    {
        ByteStreamHelper.WriteDataReference(encoder, this._data);
        encoder.WriteInt(this._count);
    }

    public void SetData(LogicData data)
    {
        this._data = data;
    }

    public LogicData GetData()
    {
        return this._data;
    }

    public void SetCount(int count)
    {
        this._count = count;
    }

    public int GetCount()
    {
        return this._count;
    }

    public void WriteToJSON(LogicJSONObject jsonObject)
    {
        jsonObject.Put("id", new LogicJSONNumber(this._data != null ? this._data.GetGlobalID() : 0));
        jsonObject.Put("cnt", new LogicJSONNumber(this._count));
    }
}
