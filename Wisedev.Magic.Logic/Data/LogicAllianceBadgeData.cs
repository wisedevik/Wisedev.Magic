using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Titam.CSV;

namespace Wisedev.Magic.Logic.Data;

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
