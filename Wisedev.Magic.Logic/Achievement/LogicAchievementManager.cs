using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.Math;

namespace Wisedev.Magic.Logic.Achievement;

public class LogicAchievementManager
{
    private LogicLevel _level;

    public LogicAchievementManager(LogicLevel level)
    {
        _level = level;
    }

    public void ObstacleCleared()
    {
        LogicAvatar homeOwnerAvatar = _level.GetHomeOwnerAvatar();
        LogicDataTable dataTable = LogicDataTables.GetTable(LogicDataType.ACHIEVEMENT);

        if (homeOwnerAvatar != null)
        {
            if (homeOwnerAvatar.IsClientAvatar())
            {
                LogicClientAvatar clientAvatar = (LogicClientAvatar)homeOwnerAvatar;
                for (int i = 0; i < dataTable.GetItemCount(); i++)
                {
                    LogicAchievementData achievementData = (LogicAchievementData)dataTable.GetItemAt(i);

                    if (achievementData.GetActionType() == 4)
                    {
                        this.RefreshAchievementProgress(clientAvatar, achievementData, clientAvatar.GetAchievementProgress(achievementData) + 1);
                    }
                }
            }
        }
    }

    public void Tick()
    {
        ;
    }

    public void RefreshAchievementProgress(LogicClientAvatar avatar, LogicAchievementData data, int value)
    {
        int state = _level.GetState();

        if (state != 5)
        {
            var progress = avatar.GetAchievementProgress(data);
            int newValue = Math.Min(value, 2000000000);
            if (progress < newValue)
            {
                avatar.SetAchievementProgress(data, newValue);
            }

            int tmp = Math.Min(newValue, data.GetActionCount());
            if (progress < tmp)
            {
                LogicClientAvatar playerAvatar = this._level.GetPlayerAvatar();
                if (playerAvatar == avatar)
                {
                    if (tmp == data.GetActionCount())
                    {
                        this._level.GetGameListener().AchievementCompleted(data);
                    }
                    else
                    {
                        this._level.GetGameListener().AchievementProgress(data);
                    }
                }
            }
        }
    }
}
