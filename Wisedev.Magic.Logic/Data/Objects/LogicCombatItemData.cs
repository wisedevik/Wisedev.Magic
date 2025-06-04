using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Data.Objects;

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
        int lngstArrSize = _upgradeLevelCount = _row.GetLongestArraySize();

        _iconExportName = new List<string>(lngstArrSize);
        _bigPicture = new List<string>(lngstArrSize);
        _upgradeTimeH = new List<int>(lngstArrSize);
        _upgradeCost = new List<int>(lngstArrSize);
        _trainingTime = new List<int>(lngstArrSize);
        _trainingCost = new List<int>(lngstArrSize);
        _laboratoryLevel = new List<int>(lngstArrSize);
        _upgradeResource = new List<LogicResourceData>(lngstArrSize);

        if (lngstArrSize >= 1)
        {
            for (int i = 0; i < lngstArrSize; i++)
            {
                _iconExportName.Add(_row.GetClampedValue("IconExportName", i));
                _bigPicture.Add(_row.GetClampedValue("BigPicture", i));
                _upgradeTimeH.Add(3600 * _row.GetClampedIntegerValue("UpgradeTimeH", i));
                _upgradeCost.Add(_row.GetClampedIntegerValue("UpgradeCost", i));
                _trainingTime.Add(_row.GetClampedIntegerValue("TrainingTime", i));
                _trainingCost.Add(_row.GetClampedIntegerValue("TrainingCost", i));
                _laboratoryLevel.Add(_row.GetClampedIntegerValue("LaboratoryLevel", i) - 1);
                _upgradeResource.Add(LogicDataTables.GetResourceByName(_row.GetClampedValue("UpgradeResource", i), null));

                if (_upgradeResource[i] == null && GetCombatItemType() != 2)
                {
                    Debugger.Error($"UpgradeResource is not defined for {GetName()}");
                }
            }
        }

        _trainingResource = LogicDataTables.GetResourceByName(_row.GetClampedValue("TrainingResource", 0), null);

        if (_trainingResource == null && GetCombatItemType() != 2)
        {
            Debugger.Error($"TrainingResource is not defined for {GetName()}");
        }

        _housingSpace = _row.GetClampedIntegerValue("HousingSpace", 0);
        _disableProduction = _row.GetClampedBooleanValue("DisableProduction", 0);
        _unitOfType = _row.GetClampedIntegerValue("UnitOfType", 0);
    }

    public bool IsProductionEnabled()
    {
        return _disableProduction;
    }

    public int GetUpgradeLevelCount()
    {
        return _upgradeLevelCount;
    }

    public string GetBigPictureExportName(int idx)
    {
        return _bigPicture[idx];
    }

    public int GetRequiredLaboratoryLevel(int idx)
    {
        return _laboratoryLevel[idx];
    }

    public int GetUpgradeTime(int idx)
    {
        return _upgradeTimeH[idx];
    }

    public LogicResourceData GetUpgradeResource(int idx)
    {
        return _upgradeResource[idx];
    }

    public int GetUpgradeCost(int idx)
    {
        return _upgradeCost[idx];
    }

    public LogicResourceData GetTrainingResource()
    {
        return _trainingResource;
    }

    public int GetTrainingCost(int idx)
    {
        return _trainingCost[idx];
    }

    public int GetTrainingTime(int idx)
    {
        return _trainingTime[idx];
    }

    public int GetHousingSpace()
    {
        return _housingSpace;
    }

    public int GetUnitOfType()
    {
        return _unitOfType;
    }

    public string GetIconExportName(int idx)
    {
        return _iconExportName[idx];
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
