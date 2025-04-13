using Wisedev.Magic.Titan.Utils;
using System.Diagnostics;

namespace Wisedev.Magic.Titan.CSV;

public class CSVNode
{
    private string _name;
    private CSVTable _table;

    public CSVNode(string[] lines, string fileName)
    {
        this._name = fileName;
        this.Load(lines);
    }

    public void Load(string[] lines)
    {
        _table = new CSVTable(this, lines.Length);

        if (lines.Length >= 2)
        {
            LogicArrayList<string> columnNames = ParseLine(lines[0]);
            LogicArrayList<string> columnTypes = ParseLine(lines[1]);

            for (int i = 0; i < columnNames.Count; i++)
            {
                _table.AddColumn(columnNames[i]);
            }

            for (int i = 0; i < columnTypes.Count; i++)
            {
                string type = columnTypes[i];
                int columnType = -1;

                if (!string.IsNullOrEmpty(type))
                {
                    if (string.Equals(type, "string", StringComparison.OrdinalIgnoreCase))
                    {
                        columnType = 0;
                    }
                    else if (string.Equals(type, "int", StringComparison.OrdinalIgnoreCase))
                    {
                        columnType = 1;
                    }
                    else if (string.Equals(type, "boolean", StringComparison.OrdinalIgnoreCase))
                    {
                        columnType = 2;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Invalid column type '{0}', column name {1}, file {2}. Expecting: int/string/boolean.", columnTypes[i], columnNames[i], _name));
                    }
                }

                _table.AddColumnType(columnType);
            }

            _table.ValidateColumnTypes();

            if (lines.Length > 2)
            {
                for (int i = 2; i < lines.Length; i++)
                {
                    LogicArrayList<string> values = ParseLine(lines[i]);

                    if (values.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(values[0]))
                        {
                            _table.CreateRow();
                        }

                        for (int j = 0; j < values.Count; j++)
                        {
                            _table.AddAndConvertValue(values[j], j);
                        }
                    }
                }
            }
        }
    }

    public LogicArrayList<string> ParseLine(string line)
    {
        bool inQuote = false;
        string readField = string.Empty;

        LogicArrayList<string> fields = new LogicArrayList<string>();

        for (int i = 0; i < line.Length; i++)
        {
            char currChar = line[i];

            if (currChar == '"')
            {
                if (inQuote)
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        readField += currChar;
                    }
                    else
                    {
                        inQuote = false;
                    }
                }
                else
                {
                    inQuote = true;
                }
            }
            else if (currChar == ',' && !inQuote)
            {
                fields.Add(readField);
                readField = string.Empty;
            }
            else
            {
                readField += currChar;
            }
        }

        fields.Add(readField);

        return fields;
    }

    public void SetName(string name)
    {
        _name = name;
    }
    
    public string GetName() 
    {
        return _name; 
    }

    public CSVTable GetTable()
    {
        return _table;
    }
}
