using Wisedev.Magic.Titam.Utils;
using System.Diagnostics;

namespace Wisedev.Magic.Titam.CSV;

public class CSVTable
{
    private CSVNode m_node;

    private readonly LogicArrayList<string> m_columnNameList;
    private readonly LogicArrayList<CSVColumn> m_columnList;
    private readonly LogicArrayList<CSVRow> m_rowList;

    private readonly int m_size;

    public CSVTable(CSVNode node, int size)
    {
        m_columnNameList = new LogicArrayList<string>();
        m_columnList = new LogicArrayList<CSVColumn>();
        m_rowList = new LogicArrayList<CSVRow>();

        m_node = node;
        m_size = size;
    }

    public void AddColumn(string name)
    {
        m_columnNameList.Add(name);
    }

    public void AddColumnType(int type)
    {
        m_columnList.Add(new CSVColumn(type, m_size));
    }

    public void AddAndConvertValue(string value, int idx)
    {
        CSVColumn column = m_columnList[idx];

        if (!string.IsNullOrEmpty(value))
        {
            switch (column.GetType())
            {
                case 0:
                    column.AddStringValue(value);
                    break;
                case 1:
                    column.AddIntegerValue(int.Parse(value));
                    break;
                case 2:
                    if (bool.TryParse(value, out bool booleanValue))
                    {
                        column.AddBooleanValue(booleanValue);
                    }
                    else
                    {
                        Console.Write(string.Format("CSVTable::addAndConvertValue invalid value '{0}' in Boolean column '{1}', {2}", value,
                                                       m_columnNameList[idx], GetFileName()));
                        column.AddBooleanValue(false);
                    }
                    break;
            }
        }
        else
        {
            column.AddEmptyValue();
        }
    }

    public string GetFileName()
    {
        return m_node.GetName();
    }

    public string GetColumnName(int idx)
    {
        return m_columnNameList[idx];
    }

    public int GetColumnCount()
    {
        return m_columnNameList.Count;
    }

    public string GetValueAt(int columnIdx, int idx)
    {
        if (columnIdx != -1)
            return m_columnList[columnIdx].GetStringValue(idx);

        return string.Empty;
    }

    public string GetValue(string name, int idx)
    {
        return GetValueAt(m_columnNameList.IndexOf(name), idx);
    }

    public int GetColumnIndexByName(string name)
    {
        return m_columnNameList.IndexOf(name);
    }

    public int GetIntegerValueAt(int columnIdx, int idx)
    {
        if (columnIdx != -1)
            return m_columnList[columnIdx].GetIntegerValue(idx);

        return 0;
    }

    public int GetIntegerValue(string name, int idx)
    {
        return GetIntegerValueAt(m_columnNameList.IndexOf(name), idx);
    }

    public bool GetBooleanValueAt(int columnIdx, int idx)
    {
        if (columnIdx != -1)
            return m_columnList[columnIdx].GetBooleanValue(idx);

        return false;
    }

    public bool GetBooleanValue(string name, int idx)
    {
        return GetBooleanValueAt(m_columnNameList.IndexOf(name), idx);
    }
    
    public CSVRow GetRowAt(int idx)
    {
        return m_rowList[idx];
    }

    public void AddRow(CSVRow row)
    {
        m_rowList.Add(row);
    }

    public int GetColumnRowCount()
    {
        return m_columnList[0].GetSize();
    }

    public int GetColumnType(int idx)
    {
        return m_columnList[idx].GetType();
    }

    public int GetRowCount()
    {
        return m_rowList.Count;
    }

    public int GetArraySizeAt(CSVRow row, int columnIdx)
    {
        if (m_rowList.Count > 0)
        {
            int rowIdx = m_rowList.IndexOf(row);

            if (rowIdx != -1)
            {
                CSVColumn column = m_columnList[columnIdx];
                return column.GetArraySize(m_rowList[rowIdx].GetRowOffset(),
                                           rowIdx + 1 >= m_rowList.Count ? column.GetSize() : m_rowList[rowIdx + 1].GetRowOffset());
            }
        }

        return 0;
    }

    public void SetStringValueAt(string value, int cIdx, int idx)
    {
        m_columnList[cIdx].SetStringValue(value, idx);
    }

    public void CreateRow()
    {
        m_rowList.Add(new CSVRow(this));
    }

    public void ColumnNamesLoaded()
    {
        m_columnList.EnsureCapacity(m_columnNameList.Count);
    }

    public void ValidateColumnTypes()
    {
        if (m_columnNameList.Count != m_columnList.Count)
            Console.WriteLine($"Column name count {m_columnNameList.Count}, column type count {m_columnList.Count}, file {GetFileName()}");
    }
}
