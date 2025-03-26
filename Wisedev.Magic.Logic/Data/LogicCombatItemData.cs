using Wisedev.Magic.Titam.CSV;
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
    private List<LogicResourceData> _trainingResource;
    private List<int> _housingSpace;
    private List<bool> _disableProduction;
    private List<int> _unitOfType;

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
        this._trainingResource = new List<LogicResourceData>(lngstArrSize);
        this._housingSpace = new List<int>(lngstArrSize);
        this._disableProduction = new List<bool>(lngstArrSize);
        this._unitOfType = new List<int>(lngstArrSize);

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

                this._trainingResource.Add(LogicDataTables.GetResourceByName(this._row.GetClampedValue("TrainingResource", i), null));

                if (this._trainingResource[i] == null && this.GetCombatItemType() != 2)
                {
                    Debugger.Error($"TrainingResource is not defined for {this.GetName()}");
                }

                this._housingSpace.Add(this._row.GetClampedIntegerValue("HousingSpace", i));
                this._disableProduction.Add(this._row.GetClampedBooleanValue("DisableProduction", i));
                this._unitOfType.Add(this._row.GetClampedIntegerValue("UnitOfType", i));

            }
        }
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

    public List<LogicResourceData> GetTrainingResource()
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

    public List<int> GetHousingSpace()
    {
        return this._housingSpace;
    }

    public List<int> GetUnitOfType()
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
