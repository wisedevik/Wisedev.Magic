using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Data.Objects;

public class LogicResourceData : LogicData
{
    private string _capFullTID;
    private string _swf;
    private string _resourceIconExportName;
    private string _hudInstanceName;
    private int _textRed;
    private int _textBlue;
    private int _textGreen;
    private bool _premiumCurrency;

    public LogicResourceData(CSVRow row, LogicDataTable table) : base(row, table)
    {
        _capFullTID = string.Empty;
        _swf = string.Empty;
        _resourceIconExportName = string.Empty;
        _hudInstanceName = string.Empty;
        _textRed = 0;
        _textBlue = 0;
        _textGreen = 0;
        _premiumCurrency = false;

        Debugger.Print($"{table.GetItemCount()}");
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        _capFullTID = _row.GetValue("CapFullTID", 0);
        _swf = _row.GetValue("SWF", 0);
        _resourceIconExportName = _row.GetValue("ResourceIconExportName", 0);
        _hudInstanceName = _row.GetValue("HudInstanceName", 0);
        _textRed = _row.GetIntegerValue("TextRed", 0);
        _textBlue = _row.GetIntegerValue("TextBlue", 0);
        _textGreen = _row.GetIntegerValue("TextGreen", 0);
        _premiumCurrency = _row.GetBooleanValue("PremiumCurrency", 0);
    }

    public string GetCapFullTID()
    {
        return _capFullTID;
    }

    public string GetSWF()
    {
        return _swf;
    }

    public string GetResourceIconExportName()
    {
        return _resourceIconExportName;
    }

    public string GetHUDInstanceName()
    {
        return _hudInstanceName;
    }

    public int GetTextRed()
    {
        return _textRed;
    }

    public int GetTextBlue()
    {
        return _textBlue;
    }

    public int GetTextGreen()
    {
        return _textGreen;
    }

    public bool IsPremiumCurrency()
    {
        return _premiumCurrency;
    }
}
