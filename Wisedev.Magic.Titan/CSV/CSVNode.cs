using Wisedev.Magic.Titam.Utils;
using System.Diagnostics;

namespace Wisedev.Magic.Titam.CSV;

public class CSVNode
{
    private string m_name;
    private CSVTable m_table;

    public CSVNode(string[] lines, string fileName)
    {
        m_name = fileName;
        Load(lines);
    }

    public void Load(string[] lines)
    {
        m_table = new CSVTable(this, lines.Length);

        if (lines.Length >= 2)
        {
            LogicArrayList<string> columnNames = ParseLine(lines[0]);
            LogicArrayList<string> columnTypes = ParseLine(lines[1]);

            for (int i = 0; i < columnNames.Count; i++)
            {
                m_table.AddColumn(columnNames[i]);
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
                        Console.WriteLine(string.Format("Invalid column type '{0}', column name {1}, file {2}. Expecting: int/string/boolean.", columnTypes[i], columnNames[i], m_name));
                    }
                }

                m_table.AddColumnType(columnType);
            }

            m_table.ValidateColumnTypes();

            if (lines.Length > 2)
            {
                for (int i = 2; i < lines.Length; i++)
                {
                    LogicArrayList<string> values = ParseLine(lines[i]);

                    if (values.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(values[0]))
                        {
                            m_table.CreateRow();
                        }

                        for (int j = 0; j < values.Count; j++)
                        {
                            m_table.AddAndConvertValue(values[j], j);
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
        m_name = name;
    }
    
    public string GetName() 
    {
        return m_name; 
    }

    public CSVTable GetTable()
    {
        return m_table;
    }
}
