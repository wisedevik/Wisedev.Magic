using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Logic.Util;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Utils;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicNpcData : LogicData
{
    private int _expLevel;
    private int _gold;
    private int _elixir;

    private List<LogicDataSlot> _unitCount;

    public LogicNpcData(CSVRow row, LogicDataTable table) : base(row, table)
    {
        _unitCount = new List<LogicDataSlot>();
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _expLevel = _row.GetIntegerValue("ExpLevel", 0);
        _gold = _row.GetIntegerValue("Gold", 0);
        _elixir = _row.GetIntegerValue("Elixir", 0);

        int unitCountSize = _row.GetArraySize("UnitType");

        if (unitCountSize > 0)
        {
            _unitCount.EnsureCapacity(unitCountSize);

            for (int i = 0; i < unitCountSize; i++)
            {
                int cnt = _row.GetIntegerValue("UnitCount", i);

                if (cnt > 0)
                    _unitCount.Add(new LogicDataSlot(LogicDataTables.GetCharacterByName(_row.GetValue("UnitType", i), this), cnt));
            }
        }
    }

    public int GetExpLevel()
    {
        return _expLevel;
    }

    public int GetGold()
    {
        return _gold;
    }

    public int GetElixir()
    {
        return _elixir;
    }

    public List<LogicDataSlot> GetClonedUnits()
    {
        List<LogicDataSlot> units = new List<LogicDataSlot>();

        for (int i = 0; i < _unitCount.Count; i++)
        {
            units.Add(_unitCount[i].Clone());
        }

        return units;
    }
}
