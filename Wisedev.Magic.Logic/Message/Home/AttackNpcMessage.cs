using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

[PiranhaMessage(AttackNpcMessage.MESSAGE_TYPE)]
public class AttackNpcMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 14134;
    private LogicNpcData _data;

    public AttackNpcMessage() : base(0)
    {
    }

    public override void Decode()
    {
        base.Decode();
        this._data = (LogicNpcData) ByteStreamHelper.ReadDataReference(this._stream, LogicDataType.NPC);
    }

    public LogicNpcData GetNpcData()
    {
        return this._data;
    }

    public override short GetMessageType()
    {
        return AttackNpcMessage.MESSAGE_TYPE;
    }
}
