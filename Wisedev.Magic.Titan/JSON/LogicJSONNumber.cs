using System.Text;

namespace Wisedev.Magic.Titan.JSON;

public class LogicJSONNumber : LogicJSONNode
{
    private int m_value;

    public LogicJSONNumber(int value)
    {
        m_value = value;
    }

    public LogicJSONNumber()
    {
        this.m_value = 0;
    }

    public int GetIntValue()
    {
        return m_value;
    }

    public void SetIntValue(int i)
    {
        m_value = i;
    }

    public override LogicJSONNodeType GetType()
    {
        return LogicJSONNodeType.NUMBER;
    }

    public override void WriteToString(StringBuilder builder)
    {
        builder.Append(m_value);
    }
}
