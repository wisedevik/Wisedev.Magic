using Wisedev.Magic.Titam.Utils;
using System.Diagnostics;
using System.Text;

namespace Wisedev.Magic.Titam.JSON;

public class LogicJSONObject : LogicJSONNode
{
    private LogicArrayList<string> m_keys;
    private LogicArrayList<LogicJSONNode> m_values;

    public LogicJSONObject()
    {
        m_keys = new LogicArrayList<string>();
        m_values = new LogicArrayList<LogicJSONNode>();
    }

    public LogicJSONObject(int capacity)
    {
        m_keys = new LogicArrayList<string>(capacity);
        m_values = new LogicArrayList<LogicJSONNode>(capacity);
    }

    public LogicJSONNode Get(string key)
    {
        int itemIndex = m_keys.IndexOf(key);

        if (itemIndex == -1)
        {
            return null;
        }

        return m_values[itemIndex];
    }

    public LogicJSONBoolean GetJSONBoolean(string key)
    {
        int itemIndex = m_keys.IndexOf(key);

        if (itemIndex == -1)
        {
            return null;
        }

        LogicJSONNode node = m_values[itemIndex];

        if (node.GetType() == LogicJSONNodeType.BOOLEAN)
        {
            return (LogicJSONBoolean)node;
        }

        Console.WriteLine(string.Format("LogicJSONObject::getJSONBoolean type is {0}, key {1}", node.GetType(), key));

        return null;
    }

    public LogicJSONNumber GetJSONNumber(string key)
    {
        int itemIndex = m_keys.IndexOf(key);

        if (itemIndex == -1)
        {
            return null;
        }

        LogicJSONNode node = m_values[itemIndex];

        if (node.GetType() == LogicJSONNodeType.NUMBER)
        {
            return (LogicJSONNumber)node;
        }

        Console.WriteLine(string.Format("LogicJSONObject::getJSONNumber type is {0}, key {1}", node.GetType(), key));

        return null;
    }

    public LogicJSONObject GetJSONObject(string key)
    {
        int itemIndex = m_keys.IndexOf(key);

        if (itemIndex == -1)
        {
            return null;
        }

        LogicJSONNode node = m_values[itemIndex];

        if (node.GetType() == LogicJSONNodeType.OBJECT)
        {
            return (LogicJSONObject)node;
        }

        Console.WriteLine(string.Format("LogicJSONObject::getJSONObject type is {0}, key {1}", node.GetType(), key));

        return null;
    }

    public LogicJSONString GetJSONString(string key)
    {
        int itemIndex = m_keys.IndexOf(key);

        if (itemIndex == -1)
        {
            return null;
        }

        LogicJSONNode node = m_values[itemIndex];

        if (node.GetType() == LogicJSONNodeType.STRING)
        {
            return (LogicJSONString)node;
        }

        Console.WriteLine(string.Format("LogicJSONObject::getJSONString type is {0}, key {1}", node.GetType(), key));

        return null;
    }

    public LogicJSONArray GetJSONArray(string key)
    {
        int itemIndex = m_keys.IndexOf(key);

        if (itemIndex == -1)
        {
            return null;
        }

        LogicJSONNode node = m_values[itemIndex];

        if (node.GetType() == LogicJSONNodeType.ARRAY)
        {
            return (LogicJSONArray)node;
        }

        Console.WriteLine(string.Format("LogicJSONObject::getJSONArray type is {0}, key {1}", node.GetType(), key));

        return null;
    }


    public override LogicJSONNodeType GetType()
    {
        return LogicJSONNodeType.OBJECT;
    }

    public void Put(string key, LogicJSONNode item)
    {
        int keyIndex = m_keys.IndexOf(key);

        if (keyIndex != -1)
        {
            Console.WriteLine(string.Format("LogicJSONObject::put already contains key {0}", key));
        }
        else
        {
            int itemIndex = m_values.IndexOf(item);

            if (itemIndex != -1)
            {
                Console.WriteLine(string.Format("LogicJSONObject::put already contains the given JSONNode pointer. Key {0}", key));
            }
            else
            {
                m_values.Add(item);
                m_keys.Add(key);
            }
        }
    }

    public void Remove(string key)
    {
        int keyIndex = m_keys.IndexOf(key);

        if (keyIndex != -1)
        {
            m_keys.Remove(keyIndex);
            m_values.Remove(keyIndex);
        }
    }

    public int GetObjectCount()
    {
        return m_values.Count;
    }

    public override void WriteToString(StringBuilder builder)
    {
        builder.Append('{');

        for (int i = 0; i < m_values.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(',');
            }

            LogicJSONParser.WriteString(m_keys[i], builder);
            builder.Append(':');
            m_values[i].WriteToString(builder);
        }

        builder.Append('}');
    }
}
