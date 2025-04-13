using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data;

public class LogicBuildingData : LogicData
{
    private int _width;
    private int _height;
    private int _upgradeLevelCnt;
    private bool _locked;
    private bool _forgesSpells;
    private int[] _housingSpace;
    private bool _bunker;
    private int[] _constructionTimes;

    public LogicBuildingData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        this._width = this._row.GetIntegerValue("Width", 0);
        this._height = this._row.GetIntegerValue("Height", 0);

        this._bunker = this._row.GetBooleanValue("Bunker", 0);

        this._locked = this._row.GetBooleanValue("Locked", 0);
        this._forgesSpells = this._row.GetBooleanValue("ForgesSpells", 0);

        this._upgradeLevelCnt = this._row.GetLongestArraySize();
        this._housingSpace = new int[this._upgradeLevelCnt];
        this._constructionTimes = new int[this._upgradeLevelCnt];

        for (int i = 0; i < this._upgradeLevelCnt; i++)
        {

            this._constructionTimes[i] = 86400 * this._row.GetClampedIntegerValue("BuildTimeD", i) +
                                              3600 * this._row.GetClampedIntegerValue("BuildTimeH", i) +
                                              60 * this._row.GetClampedIntegerValue("BuildTimeM", i) +
                                              this._row.GetIntegerValue("BuildTimeS", i);

            this._housingSpace[i] = this._row.GetClampedIntegerValue("HousingSpace", i);
        }
    }

    public int GetUpgradeLevelCnt()
    {
        return this._upgradeLevelCnt;
    }

    public bool IsLocked()
    {
        return this._locked;
    }

    public bool IsForgesSpells()
    {
        return this._forgesSpells;
    }

    public int GetUnitStorageCapacity(int level)
    {
        return this._housingSpace[level];
    }

    public int GetWidth()
    {
        return this._width;
    }

    public int GetHeight()
    {
        return this._height;
    }

    public bool IsAllianceCastle()
    {
        return this._bunker;
    }

    public int GetConstructionTime(int upgLevel)
    {
        return this._constructionTimes[upgLevel];
    }
}
