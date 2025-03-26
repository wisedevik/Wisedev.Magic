using Wisedev.Magic.Logic.Avatar.Listener;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Util;
using Wisedev.Magic.Titam.Logic;
using Wisedev.Magic.Titam.Utils;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Avatar;

public class LogicAvatar : LogicBase
{
    protected LogicAvatarChangeListener? _listener;

    protected LogicLevel _level;

    protected List<LogicDataSlot> _resourceCount;
    protected List<LogicDataSlot> _unitCount;
    protected List<LogicDataSlot> _spellsCount;
    protected List<LogicDataSlot> _resourceCap;
    protected List<LogicData> _missionCompleted;
    protected List<LogicDataSlot> _heroState;
    protected List<LogicDataSlot> _heroHealth;
    protected List<LogicDataSlot> _unitUpgrade;
    protected List<LogicDataSlot> _spellUpgrade;
    protected List<LogicDataSlot> _heroUpgrade;

    protected List<LogicUnitSlot> _allianceUnitCount;

    protected int _allianceCastleUsedCapacity;
    protected int _allianceCastleLevel;
    protected int _allianceCastleTotalCapacity;
    protected int _allianceCastleFreeCapacity;
    protected int _townHallLevel;

    public List<LogicDataSlot> ResourceCount { get { return this._resourceCount; } set { this._resourceCount = value; } }
    public List<LogicData> MissionCompleted { get { return this._missionCompleted; } set { this._missionCompleted = value; } }
    public List<LogicDataSlot> UnitCount { get { return this._unitCount; } set { this._unitCount = value; } }


    public LogicAvatar()
    {
        this.InitBase();
    }

    public void InitBase()
    {
        this._listener = new LogicAvatarChangeListener();
        this._resourceCount = new List<LogicDataSlot>();
        this._unitCount = new List<LogicDataSlot>();
        this._spellsCount = new List<LogicDataSlot>();
        this._resourceCap = new List<LogicDataSlot>();
        this._missionCompleted = new List<LogicData>();
        this._heroState = new List<LogicDataSlot>();
        this._heroHealth = new List<LogicDataSlot>();
        this._unitUpgrade = new List<LogicDataSlot>();
        this._spellsCount = new List<LogicDataSlot>();
        this._spellUpgrade = new List<LogicDataSlot>();
        this._heroUpgrade = new List<LogicDataSlot>();
        this._allianceUnitCount = new List<LogicUnitSlot>();
    }

    public void SetLevel(LogicLevel level)
    {
        this._level = level;
    }

    public void ClearDataSlotArray(List<LogicDataSlot> logicDataSlots)
    {
        if (logicDataSlots.Count >= 1)
        {
            for (int i = 0; i < logicDataSlots.Count; i++)
                logicDataSlots[i].Destruct();
            logicDataSlots.Clear();
        }
    }

    public void ClearUnitSlotArray(List<LogicUnitSlot> logicUnitSlots)
    {
        if (logicUnitSlots.Count >= 1)
        {
            for (int i = 0; i < logicUnitSlots.Count; i++)
                logicUnitSlots[i].Destruct();
            logicUnitSlots.Clear();
        }
    }

    public virtual LogicLong GetAllianceId()
    {
        return 0;
    }

    public virtual int GetAllianceBadge()
    {
        return 0;
    }

    public virtual int GetAllianceRole()
    {
        return 1;
    }

    public virtual int GetExpLevel()
    {
        return 1;
    }

    public void SetResourceCount(LogicResourceData resourceData, int cnt)
    {
        if (resourceData.IsPremiumCurrency())
        {
            Debugger.Warning("LogicAvatar.setResourceCount shouldn't be used for diamonds");
            return;
        }

        int idx = -1;

        for (int i = 0; i < this._resourceCount.Count; i++)
        {
            if (this._resourceCount[i].GetData() == resourceData)
            {
                idx = i;
                break;
            }
        }

        if (idx != -1)
        {
            this._resourceCount[idx].SetCount(cnt);
        }
        else
        {
            this._resourceCount.Add(new LogicDataSlot(resourceData, cnt));
        }

        // TODO: add DivideAvatarResourcesToStorages
    }

    public virtual int GetResourceCount(LogicResourceData data)
    {
        if (!data.IsPremiumCurrency())
        {
            int idx = -1;

            for (int i = 0; i < this._resourceCount.Count; i++)
            {
                if (this._resourceCount[i].GetData() == data)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                return this._resourceCount[idx].GetCount();
            }
        }
        else
        {
            Debugger.Warning("LogicClientAvatar.GetResourceCount shouldn't be used for diamonds");
            return 0;
        }
       
        return 0;
    }

    public virtual List<LogicDataSlot> GetResources()
    {
        return this._resourceCount;
    }

    public virtual void SetUnitCount(LogicCombatItemData data, int cnt)
    {
        if (data.GetCombatItemType() != 0)
        {
            int idx = -1;

            for (int i = 0; i < this._spellsCount.Count; i++)
            {
                if (this._spellsCount[i].GetData() == data)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                this._spellsCount[idx].SetCount(cnt);
            }
            else
            {
                this._spellsCount.Add(new LogicDataSlot(data, cnt));
            }
        }
        else
        {
            int idx = -1;

            for (int i = 0; i < this._unitCount.Count; i++)
            {
                if (this._unitCount[i].GetData() == data)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                this._unitCount[idx].SetCount(cnt);
            }
            else
            {
                this._unitCount.Add(new LogicDataSlot(data, cnt));
            }
        }
    }

    public virtual int GetUnitCount(LogicCombatItemData data)
    {
        if (data.GetCombatItemType() != 0)
        {
            int idx = -1;

            for (int i = 0; i < this._spellsCount.Count; i++)
            {
                if (this._spellsCount[i].GetData() == data)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                return this._spellsCount[idx].GetCount();
            }
        }
        else
        {
            int idx = -1;

            for (int i = 0; i < this._unitCount.Count; i++)
            {
                if (this._unitCount[i].GetData() == data)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                return this._unitCount[idx].GetCount();
            }
        }

        return 0;
    }

    public List<LogicDataSlot> GetUnits()
    {
        return this._unitCount;
    }

    public List<LogicDataSlot> GetSpells()
    {
        return this._spellsCount;
    }

    public virtual void SetCommodityCount(int type, LogicData data, int cnt)
    {
        switch (data.GetDataType())
        {
            case LogicDataType.RESOURCE:
                switch (type)
                {
                    case 0:
                        this.SetResourceCount((LogicResourceData)data, cnt);
                        break;
                    case 1:
                        this.SetResourceCap((LogicResourceData)data, cnt);
                        break;
                    default:
                        Debugger.Error("setCommodityCount - Unknown resource commodity");
                        break;
                }
                break;
            case LogicDataType.CHARACTER:
                break;
            case LogicDataType.MISSION:
                if (type == 0)
                    this.SetMissionCompleted((LogicMissionData)data, cnt != 0);
                break;
        }
    }

    public virtual void SetMissionCompleted(LogicMissionData data, bool state)
    {
        int idx = -1;

        for (int i = 0; i < this._missionCompleted.Count; i++)
        {
            if (this._missionCompleted[i] == data)
            {
                idx = i;
                break;
            }
        }

        if (state)
        {
            if (idx == -1)
            {
                this._missionCompleted.Add(data);
            }
        }
        else
        {
            if (idx != -1)
            {
                this._missionCompleted.RemoveAt(idx);
            }
        }
    }

    public virtual void SetResourceCap(LogicResourceData data, int cnt)
    {
        if (data.IsPremiumCurrency())
        {
            Debugger.Warning("LogicClientAvatar.setResourceCap shouldn't be used for diamonds");
        }
        else
        {
            int idx = -1;

            for (int i = 0; i < this._resourceCap.Count; i++)
            {
                if (this._resourceCap[i].GetData() == data)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                this._resourceCap[idx].SetCount(cnt);
            }
            else
            {
                this._resourceCap.Add(new LogicDataSlot(data, cnt));
            }
        }
    }

    public virtual void SetHeroState(LogicHeroData data, int cnt)
    {
        int idx = -1;

        for (int i = 0; i < this._heroState.Count; i++)
        {
            if (this._heroState[i].GetData() == data)
            {
                idx = i;
                break;
            }
        }

        if (idx != -1)
        {
            this._heroState[idx].SetCount(cnt);
        }
        else
        {
            this._heroState.Add(new LogicDataSlot(data, cnt));
        }
    }

    public virtual int GetHeroState(LogicHeroData data)
    {
        int idx = -1;

        for (int i = 0; i < this._heroState.Count; i++)
        {
            if (this._heroState[i].GetData() == data)
            {
                idx = i;
                break;
            }
        }

        if (idx != -1)
        {
            return this._heroState[idx].GetCount();
        }

        return 0;
    }

    public virtual void SetHeroHealth(LogicHeroData data, int cnt)
    {
        int idx = -1;

        for (int i = 0; i < this._heroHealth.Count; i++)
        {
            if (this._heroHealth[i].GetData() == data)
            {
                idx = i;
                break;
            }
        }

        if (idx != -1)
        {
            this._heroHealth[idx].SetCount(cnt);
        }
        else
        {
            this._heroHealth.Add(new LogicDataSlot(data, cnt));
        }
    }

    public virtual int GetHeroHealth(LogicHeroData data)
    {
        int idx = -1;

        for (int i = 0; i < this._heroState.Count; i++)
        {
            if (this._heroState[i].GetData() == data)
            {
                idx = i;
                break;
            }
        }

        if (idx != -1)
        {
            return this._heroState[idx].GetCount();
        }

        return 0;
    }

    public virtual void SetUnitUpgradeLevel(LogicCombatItemData data, int cnt)
    {
        int combatItemType = data.GetCombatItemType();
        int upgradeLevelCount = data.GetUpgradeLevelCount();

        if (combatItemType > 0)
        {
            if (combatItemType == 2)
            {
                if (upgradeLevelCount <= cnt)
                {
                    Debugger.Warning("LogicAvatar.setUnitUpgradeLevel - Level is out of bounds!");
                    cnt = upgradeLevelCount - 1;
                }
                
                int idx = -1;

                for (int i = 0; i < this._heroUpgrade.Count; i++)
                {
                     if (this._heroUpgrade[i].GetData() == data)
                     {
                         idx = i;
                         break;
                     }
                }

                if (idx != -1)
                {
                    this._heroUpgrade[idx].SetCount(cnt);
                }
                else
                { 
                    this._heroUpgrade.Add(new LogicDataSlot(data, cnt));
                }
            }
            else
            {
                if (upgradeLevelCount <= cnt)
                {
                    Debugger.Warning("LogicAvatar.setUnitUpgradeLevel - Level is out of bounds!");
                    cnt = upgradeLevelCount - 1;
                }

                int idx = -1;

                for (int i = 0; i < this._spellUpgrade.Count; i++)
                {
                    if (this._spellUpgrade[i].GetData() == data)
                    {
                        idx = i;
                        break;
                    }
                }

                if (idx != -1)
                {
                    this._spellUpgrade[idx].SetCount(cnt);
                }
                else
                {
                    this._spellUpgrade.Add(new LogicDataSlot(data, cnt));
                }
            }
        }
        else
        {
            if (upgradeLevelCount <= cnt)
            {
                Debugger.Warning("LogicAvatar.setUnitUpgradeLevel - Level is out of bounds!");
                cnt = upgradeLevelCount - 1;
            }

            int idx = -1;

            for (int i = 0; i < this._unitUpgrade.Count; i++)
            {
                if (this._unitUpgrade[i].GetData() == data)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                this._unitUpgrade[idx].SetCount(cnt);
            }
            else
            {
                this._unitUpgrade.Add(new LogicDataSlot(data, cnt));
            }
        }
    }

    public virtual void SetAllianceUnitCount(LogicCharacterData data, int upgLvl, int cnt)
    {
        int idx = -1;

        for (int i = 0; i < this._allianceUnitCount.Count; i++)
        {
            if (this._allianceUnitCount[i].GetData() == data && this._allianceUnitCount[i].GetLevel() == upgLvl)
            {
                idx = i; 
                break;
            }

            int housingSpace = data.GetHousingSpace();
            if (idx != -1)
            {
                this._allianceCastleUsedCapacity += (cnt - this._allianceUnitCount[idx].GetCount() * housingSpace);
                this._allianceUnitCount[idx].SetCount(cnt);
            }
            else
            {
                this._allianceUnitCount.Add(new LogicUnitSlot(data, upgLvl, cnt));
                this._allianceCastleUsedCapacity += cnt * housingSpace;
            }
        }
    }

    public virtual int GetAllianceCastleUsedCapacity()
    {
        return this._allianceCastleUsedCapacity;
    }

    public virtual void SetAllianceCastleLevel(int lvl)
    {
        this._allianceCastleLevel = lvl;
        if (lvl == -1)
            this._allianceCastleLevel = 0;
        else
        {

        }
    }
}
