using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Util;

public class LogicUnitSlot
{
    private LogicData _data;
    private int _level;
    private int _count;

    public LogicUnitSlot(LogicData data, int level, int count)
    {
        this._data = data;
        this._level = level;
        this._count = count;
    }

    public void Destruct()
    {
        this._data = null;
        this._level = 0;
        this._count = 0;
    }

    public void Encode(ChecksumEncoder encoder)
    {
        ByteStreamHelper.WriteDataReference(encoder, this._data);
        encoder.WriteInt(this._level);
        encoder.WriteInt(this._count);
    }

    public LogicData GetData()
    {
        return this._data;
    }

    public int GetLevel()
    {
        return this._level;
    }

    public int GetCount()
    {
        return this._count;
    }
     
    public void SetCount(int cnt)
    {
        this._count = cnt;
    }
}
