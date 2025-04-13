using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data;

public class LogicData
{
    protected int _globalId;

    protected string _iconSWF;
    protected string _iconExportName;
    protected string _tid;
    protected string _infoTID;

    protected CSVRow _row;
    protected readonly LogicDataTable _table;

    public int GlobalID { get { return this._globalId; } set { this._globalId = value; } }

    public LogicData(CSVRow row, LogicDataTable table)
    {
        this._row = row;
        this._table = table;
        this._globalId = Data.GlobalID.CreateGlobalID((int)table.GetTableIndex() + 1, table.GetItemCount());
    }

    public virtual void CreateReferences()
    {
        this._iconSWF = this._row.GetValue("IconSWF", 0);
        this._iconExportName = this._row.GetValue("IconExportName", 0);
        this._tid = this._row.GetValue("TID", 0);
        this._infoTID = this._row.GetValue("InfoTID", 0);
    }

    public void SetCSVRow(CSVRow row)
    {
        this._row = row;
    }

    public int GetGlobalID()
    {
        return this._globalId;
    }

    public int GetInstanceID()
    {
        return Data.GlobalID.GetInstanceID(this._globalId);
    }

    public string GetName()
    {
        return this._row.GetName();
    }

    public LogicDataType GetDataType()
    {
        return this._table.GetTableIndex();
    }

    public string GetIconSWF()
    {
        return this._iconSWF;
    }

    public string GetIconExportName()
    {
        return this._iconExportName;
    }

    public string GetTID()
    {
        return this._tid;
    }

    public string GetInfoTID()
    {
        return this._infoTID;
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