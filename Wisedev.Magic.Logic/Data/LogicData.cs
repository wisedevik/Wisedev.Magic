﻿using Wisedev.Magic.Titam.CSV;

namespace Wisedev.Magic.Logic.Data;

public class LogicData
{
    protected readonly int _globalId;

    protected string _iconSWF;
    protected string _iconExportName;
    protected string _tid;
    protected string _infoTID;

    protected CSVRow _row;
    protected readonly LogicDataTable _table;

    public LogicData(CSVRow row, LogicDataTable table)
    {
        this._row = row;
        this._table = table;
        this._globalId = GlobalID.CreateGlobalID(table.GetTableIndex() + 1, table.GetItemCount());
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
        return GlobalID.GetInstanceID(this._globalId);
    }

    public string GetName()
    {
        return this._row.GetName();
    }

    public int GetDataType()
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
