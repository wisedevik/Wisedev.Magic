using Wisedev.Magic.Logic.Util;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Utils;

namespace Wisedev.Magic.Logic.Data;

public class LogicNpcData : LogicData
{
    private int _expLevel;
    private int _gold;
    private int _elixir;

    private List<LogicDataSlot> _unitCount;

    public LogicNpcData(CSVRow row, LogicDataTable table) : base(row, table)
    {
        this._unitCount = new List<LogicDataSlot>();
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        this._expLevel = this._row.GetIntegerValue("ExpLevel", 0);
        this._gold = this._row.GetIntegerValue("Gold", 0);
        this._elixir = this._row.GetIntegerValue("Elixir", 0);

        int unitCountSize = this._row.GetArraySize("UnitType");

        if (unitCountSize > 0)
        {
            this._unitCount.EnsureCapacity(unitCountSize);

            for (int i = 0; i < unitCountSize; i++)
            {
                int cnt = this._row.GetIntegerValue("UnitCount", i);

                if (cnt > 0)
                    this._unitCount.Add(new LogicDataSlot(LogicDataTables.GetCharacterByName(this._row.GetValue("UnitType", i), this), cnt));
            }
        }
    }

    public int GetExpLevel()
    {
        return this._expLevel;
    }

    public int GetGold()
    {
        return this._gold;
    }

    public int GetElixir()
    {
        return this._elixir;
    }

    public List<LogicDataSlot> GetClonedUnits()
    {
        List<LogicDataSlot> units = new List<LogicDataSlot>();

        for (int i = 0; i < this._unitCount.Count; i++)
        {
            units.Add(this._unitCount[i].Clone());
        }

        return units;
    }
}
