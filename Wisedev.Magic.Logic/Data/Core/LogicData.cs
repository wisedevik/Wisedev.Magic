using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data.Core;

public class LogicData
{
    protected int _globalId;

    protected string _iconSWF;
    protected string _iconExportName;
    protected string _tid;
    protected string _infoTID;

    protected CSVRow _row;
    protected readonly LogicDataTable _table;

    public int GlobalID { get { return _globalId; } set { _globalId = value; } }

    public LogicData(CSVRow row, LogicDataTable table)
    {
        _row = row;
        _table = table;
        _globalId = Core.GlobalID.CreateGlobalID((int)table.GetTableIndex() + 1, table.GetItemCount());
    }

    public virtual void CreateReferences()
    {
        _iconSWF = _row.GetValue("IconSWF", 0);
        _iconExportName = _row.GetValue("IconExportName", 0);
        _tid = _row.GetValue("TID", 0);
        _infoTID = _row.GetValue("InfoTID", 0);
    }

    public void SetCSVRow(CSVRow row)
    {
        _row = row;
    }

    public int GetGlobalID()
    {
        return _globalId;
    }

    public int GetInstanceID()
    {
        return Core.GlobalID.GetInstanceID(_globalId);
    }

    public string GetName()
    {
        return _row.GetName();
    }

    public LogicDataType GetDataType()
    {
        return _table.GetTableIndex();
    }

    public string GetIconSWF()
    {
        return _iconSWF;
    }

    public string GetIconExportName()
    {
        return _iconExportName;
    }

    public string GetTID()
    {
        return _tid;
    }

    public string GetInfoTID()
    {
        return _infoTID;
    }

}

public enum LogicDataType
{
    BUILDING = 0,
    LOCALE,
    RESOURCE,
    CHARACTER,
    ANIMATION,
    PROJECTILE,
    BUILDING_CLASS,
    OBSTACLE,
    EFFECT,
    PARTICLE_EMITTER,
    EXPERIENCE_LEVEL,
    TRAP,
    ALLIANCE_BADGE,
    GLOBAL,
    TOWN_HALL_LEVEL,
    ALLIANCE_PORTAL,
    NPC,
    DECO,
    RESOURCE_PACK,
    SHIELD,
    MISSION,
    BILLING_PACKAGES,
    ACHIEVEMENT,
    SPELL = 25,
    HINT,
    HERO,
    LEAGUE,
    NEWS,
}