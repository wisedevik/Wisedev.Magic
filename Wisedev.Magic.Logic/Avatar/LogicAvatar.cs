using Wisedev.Magic.Logic.Avatar.Listener;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Util;
using Wisedev.Magic.Titam.Logic;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Avatar;

public class LogicAvatar : LogicBase
{
    protected LogicAvatarChangeListener? _listener;

    private List<LogicDataSlot> _resourceCount;
    private List<LogicDataSlot> _unitCount;
    private List<LogicDataSlot> _spellsCount;

    public LogicAvatar()
    {
        this.InitBase();
    }

    public void InitBase()
    {
        this._listener = new LogicAvatarChangeListener();
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
}
