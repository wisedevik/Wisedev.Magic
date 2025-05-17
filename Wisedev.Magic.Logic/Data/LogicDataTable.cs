using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Data;

public class LogicDataTable
{
    private LogicDataType _tableIdx;
    private string _tableName;
    private bool _isLoaded;

    protected CSVTable _table;
    protected List<LogicData> _items;

    public LogicDataTable(CSVTable table, LogicDataType tableIdx)
    {
        this._table = table;
        this._tableIdx = tableIdx;
        this._items = new List<LogicData>();

        this.LoadTable();
    }

    public void LoadTable()
    {
        for (int i = 0, j = this._table.GetRowCount(); i < j; i++ )
        {
            this._items.Add(this.CreateItem(this._table.GetRowAt(i)));
        }
    }

    public virtual void CreateReferences()
    {
        if (!this._isLoaded)
        {
            for (int i = 0; i < this._items.Count; i++)
            {
                this._items[i].CreateReferences();
            }

            this._isLoaded = true;
        }
    }

    public LogicData GetItemAt(int index)
    {
        return this._items[index];
    }

    public LogicData CreateItem(CSVRow row)
    {
        LogicData item = null;

        switch (this._tableIdx)
        {
            case LogicDataType.BUILDING:
                item = new LogicBuildingData(row, this);
                break;
            case LogicDataType.RESOURCE:
                item = new LogicResourceData(row, this);
                break;
            case LogicDataType.CHARACTER:
                item = new LogicCharacterData(row, this);
                break;
            case LogicDataType.BUILDING_CLASS:
                item = new LogicBuildingClassData(row, this);
                break;
            case LogicDataType.OBSTACLE:
                item = new LogicObstacleData(row, this);
                break;
            case LogicDataType.TRAP:
                item = new LogicTrapData(row, this);
                break;
            case LogicDataType.ALLIANCE_BADGE:
                item = new LogicAllianceBadgeData(row, this);
                break;
            case LogicDataType.GLOBAL:
                item = new LogicGlobalData(row, this);
                break;
            case LogicDataType.TOWN_HALL_LEVEL:
                item = new LogicTownhallLevelData(row, this);
                break;
            case LogicDataType.NPC:
                item = new LogicNpcData(row, this);
                break;
            case LogicDataType.DECO:
                item = new LogicDecoData(row, this);
                break;
            case LogicDataType.SHIELD:
                item = new LogicShieldData(row, this);
                break;
            case LogicDataType.MISSION:
                item = new LogicMissionData(row, this);
                break;
            case LogicDataType.SPELL:
                item = new LogicSpellData(row, this);
                break;
            case LogicDataType.HERO:
                item = new LogicHeroData(row, this);
                break;
            case LogicDataType.LEAGUE:
                item = new LogicLeagueData(row, this);
                break;

            default:
                {
                    Debugger.Error("Invalid data table id: " + this._tableIdx);
                    break;
                }
        }

        return item;
    }

    public void SetTable(CSVTable table)
    {
        this._table = table;

        for (int i = 0; i < _items.Count; i++)
        {
            this._items[i].SetCSVRow(table.GetRowAt(i));
        }
    }

    public void SetName(string name)
    {
        this._tableName = name;
    }

    public LogicDataType GetTableIndex()
    {
        return this._tableIdx;
    }

    public int GetItemCount()
    {
        return this._items.Count;
    }

    public LogicData GetItemById(int globalId)
    {
        int instanceId = GlobalID.GetInstanceID(globalId);
        if (instanceId >= 0)
        {
            return this._items[instanceId];
        }

        Debugger.Warning("LogicDataTable.GetItemById() - Instance id out of bounds!");
        return null;
    }

    public LogicData GetDataByName(string name, LogicData? caller)
    {
        if (!string.IsNullOrEmpty(name))
        {
            for (int i = 0; i < this._items.Count; i++)
            {
                LogicData data = this._items[i];
                if (data.GetName().Equals(name))
                {
                    return data;
                }
            }

            if (caller != null)
            {
                Debugger.Warning($"CSV row ({caller.GetName()}) has an invalid reference ({name})");
            }
        }

        return null;
    }

    public string GetTableName()
    {
        return this._tableName;
    }
}
