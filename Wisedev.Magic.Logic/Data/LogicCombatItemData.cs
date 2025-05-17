using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Data;

public class LogicCombatItemData : LogicData
{
    protected int _upgradeLevelCount;

    private List<string>_iconExportName;
    private List<string> _bigPicture;
    private List<int> _upgradeTimeH;
    private List<int> _upgradeCost;
    private List<int> _trainingTime;
    private List<int> _trainingCost;
    private List<int> _laboratoryLevel;
    private List<LogicResourceData> _upgradeResource;
    private LogicResourceData _trainingResource;
    private int _housingSpace;
    private bool _disableProduction;
    private int _unitOfType;

    public LogicCombatItemData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        int lngstArrSize = this._upgradeLevelCount = this._row.GetLongestArraySize();

        this._iconExportName = new List<string>(lngstArrSize);
        this._bigPicture = new List<string>(lngstArrSize);
        this._upgradeTimeH = new List<int>(lngstArrSize);
        this._upgradeCost = new List<int>(lngstArrSize);
        this._trainingTime = new List<int>(lngstArrSize);
        this._trainingCost = new List<int>(lngstArrSize);
        this._laboratoryLevel = new List<int>(lngstArrSize);
        this._upgradeResource = new List<LogicResourceData>(lngstArrSize);

        if (lngstArrSize >= 1)
        {
            for (int i = 0; i < lngstArrSize; i++)
            {
                this._iconExportName.Add(this._row.GetClampedValue("IconExportName", i));
                this._bigPicture.Add(this._row.GetClampedValue("BigPicture", i));
                this._upgradeTimeH.Add(3600 * this._row.GetClampedIntegerValue("UpgradeTimeH", i));
                this._upgradeCost.Add(this._row.GetClampedIntegerValue("UpgradeCost", i));
                this._trainingTime.Add(this._row.GetClampedIntegerValue("TrainingTime", i));
                this._trainingCost.Add(this._row.GetClampedIntegerValue("TrainingCost", i));
                this._laboratoryLevel.Add(this._row.GetClampedIntegerValue("LaboratoryLevel", i) - 1);
                this._upgradeResource.Add(LogicDataTables.GetResourceByName(this._row.GetClampedValue("UpgradeResource", i), null));

                if (this._upgradeResource[i] == null && this.GetCombatItemType() != 2)
                {
                    Debugger.Error($"UpgradeResource is not defined for {this.GetName()}");
                }
            }
        }

        this._trainingResource = LogicDataTables.GetResourceByName(this._row.GetClampedValue("TrainingResource", 0), null);

        if (this._trainingResource == null && this.GetCombatItemType() != 2)
        {
            Debugger.Error($"TrainingResource is not defined for {this.GetName()}");
        }

        this._housingSpace = this._row.GetClampedIntegerValue("HousingSpace", 0);
        this._disableProduction = this._row.GetClampedBooleanValue("DisableProduction", 0);
        this._unitOfType = this._row.GetClampedIntegerValue("UnitOfType", 0);
    }

    public bool IsProductionEnabled()
    {
        return this._disableProduction;
    }

    public int GetUpgradeLevelCount()
    {
        return this._upgradeLevelCount;
    }

    public string GetBigPictureExportName(int idx)
    {
        return this._bigPicture[idx];
    }

    public int GetRequiredLaboratoryLevel(int idx)
    {
        return this._laboratoryLevel[idx];
    }

    public int GetUpgradeTime(int idx)
    {
        return this._upgradeTimeH[idx];
    }

    public LogicResourceData GetUpgradeResource(int idx)
    {
        return this._upgradeResource[idx];
    }

    public int GetUpgradeCost(int idx)
    {
        return this._upgradeCost[idx];
    }

    public LogicResourceData GetTrainingResource()
    {
        return this._trainingResource;
    }

    public int GetTrainingCost(int idx)
    {
        return this._trainingCost[idx];
    }

    public int GetTrainingTime(int idx)
    {
        return this._trainingTime[idx];
    }

    public int GetHousingSpace()
    {
        return this._housingSpace;
    }

    public int GetUnitOfType()
    {
        return this._unitOfType;
    }

    public string GetIconExportName(int idx)
    {
        return this._iconExportName[idx];
    }

    public virtual int GetRequiredProductionHouseLevel()
    {
        return 0;
    }

    public virtual int GetCombatItemType()
    {
        return -1;
    }

    public virtual bool IsUnlockedForProductionHouseLevel(int lvl)
    {
        return false;
    }

    public virtual bool DoesHealing()
    {
        return false;
    }
}
