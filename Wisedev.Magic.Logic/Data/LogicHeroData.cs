using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data;

public class LogicHeroData : LogicCharacterData
{
    public LogicHeroData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override int GetCombatItemType()
    {
        return 2;
    }
}
