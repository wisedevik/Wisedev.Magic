using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Battle;
using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Message.Home;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Debug;
using Wisedev.Magic.Titan.Math;

namespace Wisedev.Magic.Logic.Mode;

public class LogicGameMode
{
    private int _state;
    private LogicLevel _level;
    private LogicCommandManager _commandManager;
    private bool _battleOver;
    private LogicTimer _battleTimer;
    private LogicReplay _replay;

    private int _shieldRemainingSeconds;

    public LogicGameMode()
    {
        this._level = new LogicLevel(this);
        this._commandManager = new LogicCommandManager(this._level);
    }

    public void SaveToJSON(LogicJSONObject jsonObject)
    {
        this._level.SaveToJSON(jsonObject);
    }

    public void SetShieldRemainingSeconds(int seconds)
    {
        this._shieldRemainingSeconds = this._level.GetLogicTime().GetTick() + 60 * 2;
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

            this._shieldRemainingSeconds = 60 * logicHome.GetShieldDurationSeconds();

            logicAvatar.SetLevel(this._level);
        }
    }

    public void LoadNpcAttackState(LogicClientHome home, LogicAvatar homeOwnerAvatar, LogicAvatar visitorAvatar, int secondsSinceLastSave)
    {
        if (this._state == 1)
        {
            Debugger.Error("LoadAttackState called from invalid state");
        }
        else
        {
            this._state = 2;
            if (this._battleTimer != null)
            {
                this._battleTimer.Destruct();
                this._battleTimer = null;
            }

            if (homeOwnerAvatar.IsNpcAvatar())
            {
                LogicNpcAvatar npcAvatar = (LogicNpcAvatar)homeOwnerAvatar;
                LogicNpcData npcData = npcAvatar.GetNpcData();

                // TODO: setResource logic

                this._level.SetMatchType(2, 0);
                this._level.SetHome(home);
                this._level.SetHomeOwnerAvatar(homeOwnerAvatar);
                this._level.SetVisitorAvatar(visitorAvatar);
                //TODO: LogicLevel::fastForwardTime
                //TODO: LogicLevel::loadingFinished
            }
            else
            {
                Debugger.Error("loadNpcAttackState called and home owner is not npc avatar");
            }

        }
    }

    public bool IsInAttackPreparationMode()
    {
        if (_state == 2 || _state == 5)
        {
            LogicAvatar homeOwnerAvatar = _level.GetHomeOwnerAvatar();

            if (homeOwnerAvatar.IsClientAvatar())
            {
                if (_battleTimer != null)
                {
                    int remainingSeconds;

                    remainingSeconds = Math.Max(_battleTimer.GetRemainingSeconds(_level.GetLogicTime()), 1);
                    return remainingSeconds > LogicDataTables.GetGlobals().GetAttackLengthSec();
                }
            }
        }

        return false;
    }

    public int GetRemainingAttackSeconds()
    {
        if ((this._state == 2 || this._state == 5) && !this._battleOver)
        {
            if (this._battleTimer != null)
            {
                return LogicMath.Max(this._battleTimer.GetRemainingSeconds(this._level.GetLogicTime()), 1);
            }

            return 1;
        }

        return 0;
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

        if (_level.IsInCombatState())
        {
            if (_battleTimer != null &&
                _battleTimer.GetRemainingSeconds(logicTime) == 0 ||
                _level.GetBattleEndPending())
            {
                this.SetBattleOver();
            }
        }

        logicTime.IncreaseSubTick();
    }

    public void EndAttackPreparation()
    {
        if (_battleTimer != null)
        {
            int attackLengthSec = LogicDataTables.GetGlobals().GetAttackLengthSec();
            int battleRemainingSecs = this._battleTimer.GetRemainingSeconds(this._level.GetLogicTime());

            if (battleRemainingSecs > attackLengthSec)
                _battleTimer.StartTimer(attackLengthSec, this._level.GetLogicTime());
        }
    }

    public void SetBattleOver()
    {
        if (_battleOver)
            return;

        _battleOver = true;
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

    public int GetShieldRemainingSeconds()
    {
        return this._shieldRemainingSeconds;
    }

    public enum GameModeState
    {
        Home = 1,
        AttackNpc = 2,
    }
}
