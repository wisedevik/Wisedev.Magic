using System.Security.Cryptography;
using Wisedev.Magic.Titan.CSV;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Data.String;

public class LogicStringTable
{
    public static LogicStringTable sInstance;

    private CSVTable _table;
    private Dictionary<string, CSVRow> _strings;

    public LogicStringTable(CSVTable table)
    {
        _table = table;
        _strings = new Dictionary<string, CSVRow>();

        if (table.GetRowCount() >= 1)
        {
            for (int i = 0; i < table.GetRowCount(); i++)
            {
                CSVRow rowAt = table.GetRowAt(i);
                string tid = rowAt.GetValueAt(0, 0);

                if (!_strings.ContainsKey(tid))
                {
                    _strings[tid] = rowAt;
                }
                else
                {
                    Debugger.Warning("duplicate TID in " + tid);
                }
            }
        }
    }

    public static void CreateInstance(CSVTable table)
    {
        if (sInstance == null)
            sInstance = new LogicStringTable(table);
        else
            Debugger.Warning("LogicStringTable instance already created!");
    }

    public string GetLanguageCode(int idx)
    {
        return _table.GetColumnName(idx);
    }

    public static LogicStringTable GetInstance()
    {
        return sInstance;
    }

    public string? GetString(string tid, int columnIndex)
    {
        if (!_strings.TryGetValue(tid, out CSVRow row))
        {
            Debugger.Warning($"Can't find TID: {tid}");
            return null;
        }

        string valueAt = row.GetValueAt(columnIndex, 0);
        return valueAt;
    }
}
