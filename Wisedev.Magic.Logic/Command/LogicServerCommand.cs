using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Command;

public class LogicServerCommand : LogicCommand
{
    private int _id;

    public LogicServerCommand()
    {
        this._id = -1;
    }

    public void SetId(int id)
    {
        this._id = id;
    }

    public int GetId()
    {
        return this._id;
    }

    public override void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteInt(this._id);
        base.Encode(encoder);
    }

    public override void Decode(ByteStream stream)
    {
        this._id = stream.ReadInt();
        base.Decode(stream);
    }
}
