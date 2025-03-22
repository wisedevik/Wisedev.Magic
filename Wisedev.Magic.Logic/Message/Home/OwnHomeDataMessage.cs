using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Titam.Message;

namespace Wisedev.Magic.Logic.Message.Home;

public class OwnHomeDataMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 24101;

    private LogicClientHome _logicClientHome = new();
    private LogicClientAvatar _logicClientAvatar = new();

    private int _secondsSinceLastSave;

    public OwnHomeDataMessage() : base(0)
    {
    }

    public override void Encode()
    {
        base.Encode();
        this._stream.WriteInt(_secondsSinceLastSave);
        this._logicClientHome.Encode(_stream);
        this._logicClientAvatar.Encode(_stream);
    }

    public void SetLogicClientHome(LogicClientHome logicClientHome)
    {
        this._logicClientHome = logicClientHome;
    }

    public LogicClientHome GetLogicClientHome()
    {
        return this._logicClientHome;
    }

    public void SetLogicClientAvatar(LogicClientAvatar logicClientAvatar)
    {
        this._logicClientAvatar = logicClientAvatar;
    }

    public LogicClientAvatar GetLogicClientAvatar()
    {
        return this._logicClientAvatar;
    }

    public void SetSecondsSinceLastSave(int secondsSinceLastSave)
    {
        this._secondsSinceLastSave = secondsSinceLastSave;
    }

    public int GetSecondsSinceLastSave()
    {
        return this._secondsSinceLastSave;
    }

    public override short GetMessageType()
    {
        return OwnHomeDataMessage.MESSAGE_TYPE;
    }

    public override int GetServiceNodeType()
    {
        return 10;
    }
}
