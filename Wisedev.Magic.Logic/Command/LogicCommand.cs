using Wisedev.Magic.Logic.Command.Listener;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Command;

public class LogicCommand
{
    protected int _executeSubTick;

    public LogicCommand()
    {
        _executeSubTick = -1;
    }

    public virtual void Destruct()
    {
        ;
    }

    public void SetExecuteSubTick(int tick)
    {
        _executeSubTick = tick;
    }

    public int GetExecuteSubTick()
    {
        return _executeSubTick;
    }

    public virtual int Execute(LogicLevel level)
    {
        return 0;
    }

    public virtual void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteInt(this._executeSubTick);
    }

    public virtual void Decode(ByteStream stream)
    {
        this._executeSubTick = stream.ReadInt();
    }

    public virtual int GetCommandType()
    {
        return 0;
    }

    public virtual LogicJSONObject GetJSONForReplay()
    {
        LogicJSONObject jsonObject = new LogicJSONObject();
        jsonObject.Put("t", new LogicJSONNumber(this._executeSubTick));
        return jsonObject;
    }

    public virtual void LoadFromJSON(LogicJSONObject jsonObject)
    {
        LogicJSONNumber jSONNumber = (LogicJSONNumber)jsonObject.Get("t");
        if (jSONNumber == null)
        {
            Debugger.Error("Replay - Load command from JSON failed! Execute sub tick was not found!");
            return;
        }

        this._executeSubTick = jSONNumber.GetIntValue();
    }
}
