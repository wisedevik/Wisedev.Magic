using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Util;

namespace Wisedev.Magic.Logic.Battle;

public class LogicBattleLog
{
    private LogicLevel _level;
    private bool _battleEnded;

    private List<LogicDataSlot> _unitCount;
    private List<LogicDataSlot> _spellCount;

    public LogicBattleLog(LogicLevel level)
    {
        this._level = level;

        this._unitCount = new List<LogicDataSlot>();
        this._spellCount = new List<LogicDataSlot>();
    }

    public bool GetBattleStarted()
    {
        return this._unitCount.Count + this._spellCount.Count > 0;
    }
}
