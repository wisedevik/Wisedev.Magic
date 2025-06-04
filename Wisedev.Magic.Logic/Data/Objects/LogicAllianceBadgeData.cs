using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicAllianceBadgeData : LogicData
{
    public LogicAllianceBadgeData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
    }

    public virtual bool IsValidAllianceBadge(LogicClientAvatar avatar)
    {
        return true;
    }
}
