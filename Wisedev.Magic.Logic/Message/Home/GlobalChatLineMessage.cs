using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

public class GlobalChatLineMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 24715;

    private string _message;
    private string _avatarName;
    private int _expLvl;
    private int _leagueType;
    private LogicLong _avatarId;
    private LogicLong _homeId;
    private LogicLong _allianceId;


    public GlobalChatLineMessage() : base(0)
    {
        this._allianceId = null;
    }

    public override void Encode()
    {
        base.Encode();
        this._stream.WriteString(this._message);
        this._stream.WriteString(this._avatarName);
        this._stream.WriteInt(this._expLvl);
        this._stream.WriteInt(this._leagueType);
        this._stream.WriteLong(this._avatarId);
        this._stream.WriteLong(this._homeId);
        if (this._allianceId == null)
            this._stream.WriteBoolean(false);
        else
        {
            // TODO
        }
    }

    public void SetMessage(string message)
    {
        this._message = message;
    }

    public void SetAvatarName(string avatarName)
    {
        this._avatarName = avatarName;
    }

    public void SetExpLvl(int expLvl)
    {
        this._expLvl = expLvl;
    }

    public void SetLeagueType(int leagueType)
    {
        this._leagueType = leagueType;
    }

    public void SetAvatarId(LogicLong avatarId)
    {
        this._avatarId = avatarId;
    }

    public void SetHomeId(LogicLong homeId)
    {
        this._homeId = homeId;
    }

    public void SetAllianceId(LogicLong allianceId)
    {
        this._allianceId = allianceId;
    }

    public override short GetMessageType()
    {
        return GlobalChatLineMessage.MESSAGE_TYPE;
    }
}
