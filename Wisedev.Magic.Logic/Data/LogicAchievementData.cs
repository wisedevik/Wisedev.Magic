using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data;

public class LogicAchievementData : LogicData
{
    private int _actionCount;
    private int _actionType;

    public LogicAchievementData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _actionCount = this._row.GetIntegerValue("ActionCount", 0);

        string action = this._row.GetValue("Action", 0);

        switch (action)
        {
            case "npc_stars":
                _actionType = 0;
                break;
            case "upgrade":
                _actionType = 1;
                break;
            case "victory_points":
                _actionType = 2;
                break;
            case "unit_unlock":
                _actionType = 3;
                break;
            case "clear_obstacles":
                _actionType = 4;
                break;
        }
    }

    public int GetActionCount()
    {
        return _actionCount;
    }

    public int GetActionType()
    {
        return _actionType;
    }
}
