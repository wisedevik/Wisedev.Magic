using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicEffectData : LogicData
{
    public LogicEffectData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }
}
