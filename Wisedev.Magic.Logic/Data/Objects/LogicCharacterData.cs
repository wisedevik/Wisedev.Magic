using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicCharacterData : LogicCombatItemData
{
    private List<int> _hitpoints;
    private List<int> _damage;

    public LogicCharacterData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();

        if (_upgradeLevelCount >= 1)
        {
            _hitpoints = new List<int>(_upgradeLevelCount);
            _damage = new List<int>(_upgradeLevelCount);

            for (int i = 0; i < _upgradeLevelCount; i++)
            {
                // TODO: get animation data
                _hitpoints.Add(_row.GetClampedIntegerValue("Hitpoints", i));
                _damage.Add(_row.GetClampedIntegerValue("Damage", i));
            }
        }
    }

    public override int GetCombatItemType()
    {
        return 0;
    }
}
