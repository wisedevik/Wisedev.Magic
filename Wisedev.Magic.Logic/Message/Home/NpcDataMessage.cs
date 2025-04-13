using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

public class NpcDataMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 24133;

    private int _secondsSinceLastSave;
    private string _levelJSON;
    private LogicClientAvatar _logicClientAvatar;
    private LogicNpcAvatar _logicNpcAvatar;

    public NpcDataMessage() : base(0)
    {
    }

    public override void Encode()
    {
        base.Encode();
        this._stream.WriteInt(this._secondsSinceLastSave);
        this._stream.WriteString(this._levelJSON);
        this._logicClientAvatar.Encode(_stream);
        this._logicNpcAvatar.Encode(_stream);
    }

    public override short GetMessageType()
    {
        return NpcDataMessage.MESSAGE_TYPE;
    }

    public void SetSecondsSinceLastSave(int secondsSinceLastSave)
    {
        this._secondsSinceLastSave = secondsSinceLastSave;
    }

    public void SetLevelJSON(string levelJSON)
    {
        this._levelJSON = levelJSON;
    }

    public void SetLogicClientAvatar(LogicClientAvatar avatar)
    {
        this._logicClientAvatar = avatar;
    }

    public void SetLogicNpcAvatar(LogicNpcAvatar avatar)
    {
        this._logicNpcAvatar = avatar;
    }
}
