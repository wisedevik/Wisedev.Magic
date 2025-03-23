using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Util;
using Wisedev.Magic.Titam.Logic;

namespace Wisedev.Magic.Logic.Avatar;

public class LogicAvatar : LogicBase
{
    protected LogicAvatarChangeListener? _listener;

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
}
