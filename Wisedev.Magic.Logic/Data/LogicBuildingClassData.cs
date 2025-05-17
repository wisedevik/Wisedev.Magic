using Wisedev.Magic.Titan.CSV;

namespace Wisedev.Magic.Logic.Data;

public class LogicBuildingClassData : LogicData
{
    private bool _canBuy;
    private bool _workerClass;
    private bool _townHallClass;
    private bool _wallClass;
    private bool _shopCategoryResource;
    private bool _shopCategoryArmy;
    private bool _shopCategoryDefense;

    public LogicBuildingClassData(CSVRow row, LogicDataTable table) : base(row, table)
    {
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _canBuy = _row.GetBooleanValue("CanBuy", 0);

        _workerClass = string.Equals("Worker", this.GetName());
        _townHallClass = string.Equals("Town Hall", this.GetName());
        _wallClass = string.Equals("Wall", this.GetName());
        _shopCategoryResource = _row.GetBooleanValue("ShopCategoryResource", 0);
        _shopCategoryArmy = _row.GetBooleanValue("ShopCategoryArmy", 0);
        _shopCategoryDefense = _row.GetBooleanValue("ShopCategoryDefense", 0);
    }

    public bool CanBuy()
    {
        return _canBuy;
    }

    public bool IsWorkerClass()
    {
        return _workerClass;
    }

    public bool IsTownHallClass()
    {
        return _townHallClass;
    }

    public bool IsWallClass()
    {
        return _wallClass;
    }

    public bool IsShopCategoryResource()
    {
        return _shopCategoryResource;
    }

    public bool IsShopCategoryArmy()
    {
        return _shopCategoryArmy;
    }

    public bool IsShopCategoryDefense()
    {
        return _shopCategoryDefense;
    }
}
