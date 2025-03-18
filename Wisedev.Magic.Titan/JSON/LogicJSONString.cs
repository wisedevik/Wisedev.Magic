using System.Text;

namespace Wisedev.Magic.Titam.JSON;

public class LogicJSONString : LogicJSONNode
{
    private string m_value;

    public LogicJSONString(string value)
    {
        m_value = value;
    }

    public string GetStringValue()
    {
        return m_value;
    }

    public override LogicJSONNodeType GetType()
    {
        return LogicJSONNodeType.STRING;
    }

    public override void WriteToString(StringBuilder builder)
    {
        LogicJSONParser.WriteString(m_value, builder);
    }
}
