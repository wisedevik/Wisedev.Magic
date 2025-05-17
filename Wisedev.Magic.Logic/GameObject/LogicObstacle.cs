using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Logic.Util;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicObstacle : LogicGameObject
{
    private LogicTimer _clearingTimer;

    public LogicObstacle(LogicObstacleData data, LogicLevel level) : base(data, level)
    {
        _clearingTimer = null;
    }

    public LogicObstacleData GetObstacleData()
    {
        return (LogicObstacleData)this._data;
    }

    public int GetRemainingClearingTime()
    {
        if (_clearingTimer != null)
        {
            return _clearingTimer.GetRemainingSeconds(_level.GetLogicTime());
        }
        return 0;
    }

    public bool IsClearingOnGoing()
    {
        return _clearingTimer != null;
    }

    public bool IsFadingOut()
    {
        return false;
    }

    public bool SpeedUpClearing()
    {
        if (_clearingTimer != null)
        {
            LogicClientAvatar playerAvatar = this._level.GetPlayerAvatar();
            int speedUpCost = LogicGamePlayUtil.GetSpeedUpCost(_clearingTimer.GetRemainingSeconds(_level.GetLogicTime()), 0);

            if (playerAvatar.HasEnoughDiamonds(speedUpCost, true, _level))
            {
                playerAvatar.UseDiamonds(speedUpCost);
                this.ClearingFinished(false);
            }

            return true;
        }
        return false;
    }


    public void ClearingFinished(bool ignoreState)
    {
        int state = this._level.GetState();

        if (state == 1 || !LogicDataTables.GetGlobals().CompleteConstructionsOnlyHome() && ignoreState)
        {
            if (_level.GetHomeOwnerAvatar().IsClientAvatar())
            {
                LogicObstacleData obstacleData = this.GetObstacleData();

                if (!obstacleData.IsTombstone())
                    _level.GetAchievementManager().ObstacleCleared();

                _level.GetWorkerManager().DeallocateWorker(this);
            }
        }
    }

    public override int GetGameObjectType()
    {
        return 3;
    }
}
