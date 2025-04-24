using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

[PiranhaMessage(AskForAvatarProfileMessage.MESSAGE_TYPE)]
public class AskForAvatarProfileMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 14325;

    private LogicLong _avatarId;
    private LogicLong _homeId;
    private LogicLong _allianceId;


    public AskForAvatarProfileMessage() : base(0)
    {
        this._avatarId = new LogicLong();
        this._homeId = new LogicLong();
        this._allianceId = new LogicLong();
    }

    public LogicLong GetAvatarId()
    {
        return this._avatarId;
    }

    public LogicLong GetHomeId()
    {
        return this._homeId;
    }

    public LogicLong GetAllianceId()
    {
        return this._allianceId;
    }

    public override void Decode()
    {
        base.Decode();
        this._avatarId = this._stream.ReadLong();
        this._homeId = this._stream.ReadLong();
        if (this._stream.ReadBoolean())
            this._allianceId = this._stream.ReadLong();
    }

    public override short GetMessageType()
    {
        return AskForAvatarProfileMessage.MESSAGE_TYPE;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}
