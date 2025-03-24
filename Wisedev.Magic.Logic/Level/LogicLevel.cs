using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Mode;
using Wisedev.Magic.Logic.Time;

namespace Wisedev.Magic.Logic.Level;

public class LogicLevel
{
    private LogicGameListener _gameListener;

    private LogicGameMode _gameMode;
    private LogicTime _logicTime;

    private int _lastSeenNews;

    private LogicAvatar _visitorAvatar;
    private LogicAvatar _homeOwnerAvatar;

    public LogicLevel(LogicGameMode gameMode)
    {
        _gameMode = gameMode;
        this._logicTime = new LogicTime();
    }

    public int GetState()
    {
        return _gameMode.GetState();
    }

    public LogicTime GetLogicTime()
    {
        return this._logicTime;
    }

    public void Tick()
    {

    }

    public void SetLastSeenNews(int lastSeenNews)
    {
        this._lastSeenNews = lastSeenNews;
    }

    public void SetGameListener(LogicGameListener gameListener)
    {
        this._gameListener = gameListener;
    }

    public LogicGameListener GetGameListener()
    {
        return this._gameListener;
    }

    public LogicClientAvatar GetPlayerAvatar()
    {
        if (this._gameMode.GetState() == 1
            || this._gameMode.GetState() == 3)
                return (LogicClientAvatar)this._homeOwnerAvatar;

        return (LogicClientAvatar)this._visitorAvatar;
    }
}
