using Wisedev.Magic.Titam.CSV;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Data;

public class LogicGlobals : LogicDataTable
{
    private int _startingDiamonds;
    private int _startingGold;
    private int _startingElexir;

    public LogicGlobals(CSVTable table, LogicDataType tableIdx) : base(table, tableIdx)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        this._startingDiamonds = this.GetGlobalData("STARTING_DIAMONDS").GetNumberValue();
        this._startingGold = this.GetGlobalData("STARTING_GOLD").GetNumberValue();
        this._startingElexir = this.GetGlobalData("STARTING_ELIXIR").GetNumberValue();
    }

    public LogicGlobalData? GetGlobalData(string name)
    {
        LogicGlobalData data = (LogicGlobalData)this.GetDataByName(name, null);
        if (data == null)
        {
            Debugger.Error($"Unable to find global: {name}");
            return null;
        }

        return data;
    }

    public int GetStartingDiamonds()
    {
        return this._startingDiamonds;
    }

    public int GetStartingGold()
    {
        return this._startingGold;
    }

    public int GetStartingElexir()
    {
        return this._startingElexir;
    }
}
