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
        this._allianceId = 0;
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
        if (this._allianceId == 0)
            this._stream.WriteBoolean(false);
        else
        {
            // TODO
        }
    }

    public override void Decode()
    {
        base.Decode();
        this._message = this._stream.ReadString();
        this._avatarName = this._stream.ReadString();
        this._expLvl = this._stream.ReadInt();
        this._leagueType = this._stream.ReadInt();
        this._avatarId = this._stream.ReadLong();
        this._homeId = this._stream.ReadLong();
    }

    public string GetMessage()
    {
        return this._message;
    }

    public string GetAvatarName()
    {
        return this._avatarName;
    }

    public int GetExpLvl()
    {
        return this._expLvl;
    }

    public int GetLeagueType()
    {
        return this._leagueType;
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

    public override string ToString()
    {
        return $"{this._message}, {this._avatarName}, {this._expLvl}, {this._leagueType}, {this._avatarId}, {this._homeId}";
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
