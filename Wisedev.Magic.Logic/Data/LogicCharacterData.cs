using Wisedev.Magic.Titam.CSV;

namespace Wisedev.Magic.Logic.Data;

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

        if (this._upgradeLevelCount >= 1)
        {
            this._hitpoints = new List<int>(this._upgradeLevelCount);
            this._damage = new List<int>(this._upgradeLevelCount);

            for (int i = 0; i < this._upgradeLevelCount; i++)
            {
                // TODO: get animation data
                this._hitpoints.Add(this._row.GetClampedIntegerValue("Hitpoints", i));
                this._damage.Add(this._row.GetClampedIntegerValue("Damage", i));
            }
        }
    }
}
