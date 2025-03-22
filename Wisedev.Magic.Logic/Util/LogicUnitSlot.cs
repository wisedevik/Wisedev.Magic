using Wisedev.Magic.Titam.DataStream;

namespace Wisedev.Magic.Logic.Util;

public class LogicUnitSlot
{
    private int _data;
    private int _level;
    private int _count;

    public LogicUnitSlot(int data, int level, int count)
    {
        this._data = data;
        this._level = level;
        this._count = count;
    }

    public void Destruct()
    {
        this._data = 0;
        this._level = 0;
        this._count = 0;
    }

    public void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteInt(this._data);
        encoder.WriteInt(this._level);
        encoder.WriteInt(this._count);
    }
}
