using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Mode;

public class LogicGameMode
{
    private int _state;
    private LogicLevel _level;
    private LogicCommandManager _commandManager;
    private bool _battleOver;

    public LogicGameMode()
    {
        this._level = new LogicLevel(this);
        this._commandManager = new LogicCommandManager(this._level);
    }

    public void LoadHomeState(LogicClientHome home, LogicAvatar avatar, int value)
    {

    }

    public void UpdateOneSubTick()
    {
        LogicTime logicTime = this._level.GetLogicTime();

        this.SubTick();

        logicTime.IncreaseSubTick();

        if (logicTime.IsFullTick())
        {
            this._level.Tick();
        }
    }

    public void SubTick()
    {
        this._commandManager.SubTick();
    }

    public int GetState()
    {
        return this._state;
    }

    public LogicLevel GetLevel()
    {
        return this._level;
    }

    public LogicCommandManager GetCommandManager()
    {
        return this._commandManager;
    }
}
