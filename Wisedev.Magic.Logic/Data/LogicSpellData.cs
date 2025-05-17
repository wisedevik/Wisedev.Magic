using Wisedev.Magic.Logic.Util;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data;

public class LogicSpellData : LogicCombatItemData
{
    private int[] _damage;
    private int[] _boostTimeMS;
    private int[] _numberOfHits;

    public LogicSpellData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();

        int cnt = this._upgradeLevelCount;

        _damage = new int[cnt];
        _boostTimeMS = new int[cnt];
        _numberOfHits = new int[cnt];

        if (this._upgradeLevelCount > 0)
        {
            for (int i = 0; i < cnt; i++)
            {
                _damage[i] = LogicGamePlayUtil.DPSToSingleHit(this._row.GetClampedIntegerValue("Damage", i), 1000);
                _boostTimeMS[i] = this._row.GetClampedIntegerValue("BoostTimeMS", i);
                _numberOfHits[i] = this._row.GetClampedIntegerValue("NumberOfHits", i);
            }
        }
    }

    public int GetDamage(int level)
    {
        return _damage[level];
    }

    public int GetBoostTimeMS(int level)
    {
        return _boostTimeMS[level];
    }

    public int GetNumberOfHits(int level)
    {
        return _numberOfHits[level];
    }
}
