namespace Wisedev.Magic.Titan.CSV;

public class CSVRow
{
    private CSVTable m_table;
    private int m_rowOffset;

    public CSVRow(CSVTable table)
    {
        m_table = table;
        m_rowOffset = table.GetColumnRowCount();
    }

    public string GetValueAt(int columnIdx, int idx)
    {
        return m_table.GetValueAt(columnIdx, m_rowOffset + idx);
    }

    public int GetLongestArraySize()
    {
        int longestArraySize = 1;

        for (int i = m_table.GetColumnCount() - 1; i > 0; i--)
        {
            int arraySizeAt = m_table.GetArraySizeAt(this, i);
            if (arraySizeAt > longestArraySize)
                longestArraySize = arraySizeAt;
        }

        return longestArraySize;
    }

    public int GetColumnCount()
    {
        return m_table.GetColumnCount();
    }

    public int GetArraySizeAt(int idx)
    {
        return m_table.GetArraySizeAt(this, idx);
    }

    public string GetValue(string name, int idx)
    {
        return m_table.GetValue(name, m_rowOffset + idx);
    }

    public string GetClampedValue(string name, int idx)
    {
        int columnIdx = m_table.GetColumnIndexByName(name);

        if (columnIdx != -1)
        {
            int arraySize = m_table.GetArraySizeAt(this, columnIdx);

            if (arraySize >= 1 && arraySize <= idx)
                idx = arraySize - 1;

            return m_table.GetValueAt(columnIdx, m_rowOffset + idx);
        }

        return string.Empty;
    }

    public int GetColumnIndexByName(string name)
    {
        return m_table.GetColumnIndexByName(name);
    }

    public int GetIntegerValueAt(int cIdx, int idx)
    {
        return m_table.GetIntegerValueAt(cIdx, m_rowOffset + idx);
    }

    public int GetIntegerValue(string name, int idx)
    {
        return m_table.GetIntegerValue(name, m_rowOffset + idx);
    }

    public int GetClampedIntegerValue(string name, int idx)
    {
        int columnIdx = m_table.GetColumnIndexByName(name);
        if (columnIdx != -1)
        {
            int arraySize = m_table.GetArraySizeAt(this, columnIdx);

            if (arraySize >= 1 && arraySize <= idx)
                idx = arraySize - 1;

            return m_table.GetIntegerValueAt(columnIdx, m_rowOffset + idx);
        }

        return 0;
    }

    public bool GetBooleanValueAt(int cIdx, int idx)
    {
        return m_table.GetBooleanValueAt(cIdx, m_rowOffset + idx);
    }

    public bool GetBooleanValue(string name, int idx)
    {
        return m_table.GetBooleanValue(name, m_rowOffset + idx);
    }

    public bool GetClampedBooleanValue(string name, int idx)
    {
        int columnIdx = m_table.GetColumnIndexByName(name);

        if (columnIdx != -1)
        {
            int arraySize = m_table.GetArraySizeAt(this, columnIdx);

            if (arraySize >= 1 && arraySize <= idx)
                idx = arraySize - 1;

            return m_table.GetBooleanValueAt(columnIdx, m_rowOffset + idx);
        }

        return false;
    }

    public int GetArraySize(string column)
    {
        int columnIndex = GetColumnIndexByName(column);

        if (columnIndex == -1)
        {
            return 0;
        }

        return m_table.GetArraySizeAt(this, columnIndex);
    }

    public int GetRowOffset()
    {
        return m_rowOffset;
    }

    public int GetRowCount()
    {
        return -1;
    }

    public string GetName()
    {
        return m_table.GetValueAt(0, m_rowOffset);
    }

    public CSVTable GetTable()
    {
        return m_table;
    }

    public void SetStringValueAt(string value, int cIdx, int idx)
    {
        m_table.SetStringValueAt(value, cIdx, idx);
    }


}
