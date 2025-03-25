using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Message.Home;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Mode;

public class LogicGameMode
{
    private int _state;
    private LogicLevel _level;
    private LogicCommandManager _commandManager;
    private bool _battleOver;

    private int _shieldTime;

    public LogicGameMode()
    {
        this._level = new LogicLevel(this);
        this._commandManager = new LogicCommandManager(this._level);
    }

    public void LoadHomeState(LogicClientHome logicHome, LogicAvatar logicAvatar, int secondsSinceLastSave)
    {
        if (logicHome != null)
        {
            this._state = 1;

            this._level.SetHome(logicHome);
            this._level.SetHomeOwnerAvatar(logicAvatar);
            // TODO: this._level.FastForwardTime()
            // TODO: this._level.LoadingFinished()

            logicAvatar.SetLevel(this._level);

            this._shieldTime = 60 * logicHome.GetShieldDurationSeconds();
        }
    }

    public void UpdateOneSubTick()
    {
        LogicTime logicTime = this._level.GetLogicTime();

        if (this._state != 2 || !this._battleOver)
        {
            this._commandManager.SubTick();
            this._level.SubTick();

            if (logicTime.IsFullTick())
            {
                this._level.Tick();
            }
        }

        logicTime.IncreaseSubTick();
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
