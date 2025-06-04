using Wisedev.Magic.Logic.Data.Objects;
using Wisedev.Magic.Logic.Level;

namespace Wisedev.Magic.Logic.Mission;

public class LogicMission
{
    private LogicMissionData _missionData;
    private LogicLevel _level;

    public LogicMission(LogicMissionData missionData, LogicLevel level)
    {
        _missionData = missionData;
        _level = level;
    }
}
