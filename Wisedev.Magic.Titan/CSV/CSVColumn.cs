using Wisedev.Magic.Titam.Utils;

namespace Wisedev.Magic.Titam.CSV;

public class CSVColumn
{
    private int m_type;

    private LogicArrayList<string> m_stringValues; // type: 0
    private LogicArrayList<int> m_integerValues; // type: 1
    private LogicArrayList<byte> m_booleanValues; // type: 2

    public CSVColumn(int type, int size)
    {
        m_type = type;

        m_stringValues = new LogicArrayList<string>();
        m_integerValues = new LogicArrayList<int>();
        m_booleanValues = new LogicArrayList<byte>();


        switch (type)
        {
            case 0:
                m_stringValues.EnsureCapacity(size);
                break;
            case 1:
                m_integerValues.EnsureCapacity(size);
                break;
            case 2:
                m_booleanValues.EnsureCapacity(size);
                break;
            default:
                Console.WriteLine("Invalid CSVColumn type");
                break;
        }
    }

    public void AddStringValue(string value)
    {
        m_stringValues.Add(value);
    }

    public void AddIntegerValue(int value)
    {
        m_integerValues.Add(value);
    }

    public void AddBooleanValue(bool value)
    {
        m_booleanValues.Add((byte) (value ? 1 : 0));
    }

    public void SetIntegerValue(int value, int idx)
    {
        m_integerValues[idx] = value;
    }

    public void SetBooleanValue(byte value, int idx)
    {
        m_booleanValues[idx] = value;
    }

    public void SetStringValue(string value, int idx)
    {
        m_stringValues[idx] = value;
    }

    public string GetStringValue(int idx)
    {
        return m_stringValues[idx];
    }

    public int GetIntegerValue(int idx)
    {
        return m_integerValues[idx];
    }

    public bool GetBooleanValue(int index)
    {
        return m_booleanValues[index] == 1;
    }

    public void AddEmptyValue()
    {
        switch (m_type)
        {
            case 0:
                m_stringValues.Add(string.Empty);
                break;
            case 1:
                m_integerValues.Add(0x7FFFFFFF);
                break;
            case 2:
                m_booleanValues.Add(0x2);
                break;
        }
    }

    public int GetArraySize(int startOffset, int endOffset)
    {
        switch (m_type)
        {
            default:
                for (int i = endOffset - 1; i + 1 > startOffset; i--)
                {
                    if (m_stringValues[i].Length > 0)
                    {
                        return i - startOffset + 1;
                    }
                }

                break;
            case 1:
                for (int i = endOffset - 1; i + 1 > startOffset; i--)
                {
                    if (m_integerValues[i] != 0x7FFFFFFF)
                    {
                        return i - startOffset + 1;
                    }
                }

                break;

            case 2:
                for (int i = endOffset - 1; i + 1 > startOffset; i--)
                {
                    if (m_booleanValues[i] != 0x2)
                    {
                        return i - startOffset + 1;
                    }
                }

                break;
        }

        return 0;
    }

    public int GetSize()
    {
        switch (m_type)
        {
            case -1:
            case 0:
                return m_stringValues.Count;
            case 1:
                return m_integerValues.Count;
            case 2:
                return m_booleanValues.Count;
            default:
                return 0;
        }
    }

    public int GetType()
    {
        return m_type;
    }
}
