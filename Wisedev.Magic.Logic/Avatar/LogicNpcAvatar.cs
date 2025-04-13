using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Avatar;

public class LogicNpcAvatar : LogicAvatar
{
    private LogicNpcData _data;

    public void SetNpcData(LogicNpcData data)
    {
        this._data = data;

        this.SetResourceCount(LogicDataTables.GetGoldData(), this._data.GetGold());
        this.SetResourceCount(LogicDataTables.GetElixirData(), this._data.GetElixir());

        if (this._allianceUnitCount.Count != 0)
        {
            this.ClearUnitSlotArray(this._allianceUnitCount);
            this._allianceUnitCount = null;
        }

        if (this._unitCount.Count != 0)
        {
            this.ClearDataSlotArray(this._unitCount);
            this._unitCount = null;
        }

        this._allianceUnitCount = new List<Util.LogicUnitSlot>();
        this._unitCount = this._data.GetClonedUnits();

    }

    public LogicNpcData GetNpcData()
    {
        return this._data;
    }

    public override void Encode(ChecksumEncoder encoder)
    {
        base.Encode(encoder);
        ByteStreamHelper.WriteDataReference(encoder, this._data);
    }

    public override int GetExpLevel()
    {
        return this._data.GetExpLevel();
    }

    public override bool IsNpcAvatar()
    {
        return true;
    }
}
