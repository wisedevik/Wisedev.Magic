using Wisedev.Magic.Titam.CSV;

namespace Wisedev.Magic.Logic.Data;

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
        this._capFullTID = string.Empty;
        this._swf = string.Empty;
        this._resourceIconExportName = string.Empty;
        this._hudInstanceName = string.Empty;
        this._textRed = 0;
        this._textBlue = 0;
        this._textGreen = 0;
        this._premiumCurrency = false;
    }

    public override void CreateReferences()
    {
        base.CreateReferences();
        this._capFullTID = this._row.GetValue("CapFullTID", 0);
        this._swf = this._row.GetValue("SWF", 0);
        this._resourceIconExportName = this._row.GetValue("ResourceIconExportName", 0);
        this._hudInstanceName = this._row.GetValue("HudInstanceName", 0);
        this._textRed = this._row.GetIntegerValue("TextRed", 0);
        this._textBlue = this._row.GetIntegerValue("TextBlue", 0);
        this._textGreen = this._row.GetIntegerValue("TextGreen", 0);
        this._premiumCurrency = this._row.GetBooleanValue("PremiumCurrency", 0);
    }

    public string GetCapFullTID()
    {
        return this._capFullTID;
    }

    public string GetSWF()
    {
        return this._swf;
    }

    public string GetResourceIconExportName()
    {
        return this._resourceIconExportName;
    }

    public string GetHUDInstanceName()
    {
        return this._hudInstanceName;
    }

    public int GetTextRed()
    {
        return this._textRed;
    }

    public int GetTextBlue()
    {
        return this._textBlue;
    }

    public int GetTextGreen()
    {
        return this._textGreen;
    }

    public bool IsPremiumCurrency()
    {
        return this._premiumCurrency;
    }
}
