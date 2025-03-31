using Wisedev.Magic.Titam.Utils;
using System.Diagnostics;
using System.Text;

namespace Wisedev.Magic.Titam.JSON;

public class LogicJSONArray : LogicJSONNode
{
    private readonly List<LogicJSONNode> m_items;

    public LogicJSONArray()
    {
        m_items = new List<LogicJSONNode>(20);
    }

    public LogicJSONArray(int capacity)
    {
        m_items = new List<LogicJSONNode>(capacity);
    }

    public LogicJSONNode Get(int idx)
    {
        return m_items[idx];
    }

    public void Add(LogicJSONNode item)
    {
        m_items.Add(item);
    }

    public LogicJSONArray GetJSONArray(int index)
    {
        LogicJSONNode node = m_items[index];

        if (node.GetType() != LogicJSONNodeType.ARRAY)
        {
            Console.WriteLine(string.Format("LogicJSONObject::getJSONArray wrong type {0}, index {1}", node.GetType(), index));
            return null;
        }

        return (LogicJSONArray)node;
    }

    public LogicJSONBoolean GetJSONBoolean(int index)
    {
        LogicJSONNode node = m_items[index];

        if (node.GetType() != LogicJSONNodeType.BOOLEAN)
        {
            Console.WriteLine(string.Format("LogicJSONObject::getJSONBoolean wrong type {0}, index {1}", node.GetType(), index));
            return null;
        }

        return (LogicJSONBoolean)node;
    }

    public LogicJSONNumber GetJSONNumber(int index)
    {
        LogicJSONNode node = m_items[index];

        if (node.GetType() != LogicJSONNodeType.NUMBER)
        {
            Console.WriteLine(string.Format("LogicJSONObject::getJSONNumber wrong type {0}, index {1}", node.GetType(), index));
            return null;
        }

        return (LogicJSONNumber)node;
    }

    public LogicJSONObject GetJSONObject(int index)
    {
        LogicJSONNode node = m_items[index];

        if (node.GetType() != LogicJSONNodeType.OBJECT)
        {
            Console.WriteLine("LogicJSONObject::getJSONObject wrong type " + node.GetType() + ", index " + index);
            return null;
        }

        return (LogicJSONObject)node;
    }

    public LogicJSONString GetJSONString(int index)
    {
        LogicJSONNode node = m_items[index];

        if (node.GetType() != LogicJSONNodeType.STRING)
        {
            Console.WriteLine(string.Format("LogicJSONObject::getJSONString wrong type {0}, index {1}", node.GetType(), index));
            return null;
        }

        return (LogicJSONString)node;
    }

    public int Size()
    {
        return m_items.Count;
    }

    public override LogicJSONNodeType GetType()
    {
        return LogicJSONNodeType.ARRAY;
    }

    public override void WriteToString(StringBuilder builder)
    {
        builder.Append('[');

        for (int i = 0; i < m_items.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(',');
            }

            m_items[i].WriteToString(builder);
        }

        builder.Append(']');
    }
}
