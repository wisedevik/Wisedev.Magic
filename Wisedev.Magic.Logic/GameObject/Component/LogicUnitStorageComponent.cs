using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Util;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Debug;
using System;

namespace Wisedev.Magic.Logic.GameObject.Component;

public class LogicUnitStorageComponent : LogicComponent
{
    private int _storageType;
    private int _maxCapacity;
    private List<LogicUnitSlot> _slots;

    public LogicUnitStorageComponent(LogicGameObject gameObject, int capacity) : base(gameObject)
    {
        this._maxCapacity = capacity;
        this._slots = new List<LogicUnitSlot>();

        if (gameObject.GetGameObjectType() == 0)
        {
            this._storageType = ((LogicBuilding)gameObject).GetBuildingData().IsForgesSpells() ? 1 : 0;
        }
    }

    public void AddUnitImpl(LogicCombatItemData data, int level)
    {
        if (data != null)
        {
            // TODO: Add this.CanAddUnit(data);
            int idx = -1;

            for (int i = 0; i < this._slots.Count; i++)
            {
                LogicUnitSlot slot = this._slots[i];
                if (slot.GetData().GlobalID == data.GlobalID && slot.GetLevel() == level)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                this._slots[idx].SetCount(this._slots[idx].GetCount() + 1);
            }
            else
            {
                this._slots.Add(new LogicUnitSlot(data, level, 1));
                Console.WriteLine($"added: {data.GlobalID}");
            }
        }
    }

    public void AddUnit(LogicCombatItemData data)
    {
        this.AddUnitImpl(data, -1);
    }

    public override void Save(LogicJSONObject jsonObject)
    {
        LogicJSONArray unitArray = new LogicJSONArray();
        LogicJSONNumber storageTypeObject = new LogicJSONNumber(this._storageType);

        for (int i = 0; i < this._slots.Count; i++)
        {
            LogicUnitSlot slot = this._slots[i];

            if (slot.GetData() != null && slot.GetCount() > 0)
            {
                if (slot.GetLevel() != -1)
                {
                    Debugger.Error("Invalid unit level.");
                }

                LogicJSONArray unitObject = new LogicJSONArray();
                unitObject.Add(new LogicJSONNumber(slot.GetData().GetGlobalID()));
                unitObject.Add(new LogicJSONNumber(slot.GetCount()));
                unitArray.Add(unitObject);
            }
        }

        jsonObject.Put("units", unitArray);
        jsonObject.Put("storage_type", storageTypeObject);
    }

    public override void Load(LogicJSONObject jsonObject)
    {
        Debugger.Print("LogicUnitStorageComponent.Load called!");
        LogicJSONArray unitArray = jsonObject.GetJSONArray("units");

        if (unitArray != null)
        {
            if (this._slots.Count > 0)
                Debugger.Error("LogicUnitStorageComponent.Load - Unit array size > 0!");

            for (int i = 0, size = unitArray.Size(); i < size; i++)
            {
                LogicJSONArray unitObject = unitArray.GetJSONArray(i);

                if (unitObject != null)
                {
                    LogicJSONNumber dataObject = unitObject.GetJSONNumber(0);
                    LogicJSONNumber countObject = unitObject.GetJSONNumber(1);
                    if (dataObject != null)
                    {
                        if (countObject != null)
                        {
                            LogicData data = LogicDataTables.GetDataById(dataObject.GetIntValue(),
                                                                             this._storageType != 0 ? LogicDataType.SPELL : LogicDataType.CHARACTER);

                            if (data == null)
                            {
                                Debugger.Error("LogicUnitStorageComponent.Load - Character data is NULL!");
                            }

                            this._slots.Add(new LogicUnitSlot(data, -1, countObject.GetIntValue()));
                            foreach (LogicUnitSlot slot in this._slots)
                            {
                                Console.WriteLine($"{slot.GetData().GetGlobalID()}");
                                Console.WriteLine($"{slot.GetCount()}");
                            }
                        }
                    }
                }
            }
        }
    }

    public override int GetComponentType()
    {
        return 0;
    }
}
