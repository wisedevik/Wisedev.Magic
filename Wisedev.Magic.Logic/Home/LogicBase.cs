using Wisedev.Magic.Titam.DataStream;

namespace Wisedev.Magic.Logic.Home;

public class LogicBase
{
    private int _logicDataVersion;

    public LogicBase()
    {
        this._logicDataVersion = 0;
    }

    public LogicBase(int dataV)
    {
        this._logicDataVersion = dataV;
    }

    public int GetLogicDataVersion()
    {
        return this._logicDataVersion;
    }

    public void SetLogicDataVersion(int dataV)
    {
        this._logicDataVersion = dataV;
    }

    public virtual void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteInt(this._logicDataVersion);
    }

    public virtual void Decode(ByteStream stream)
    {
        this._logicDataVersion = stream.ReadInt();
    }

    public virtual void Destruct()
    {
        this._logicDataVersion = 0;
    }
}
