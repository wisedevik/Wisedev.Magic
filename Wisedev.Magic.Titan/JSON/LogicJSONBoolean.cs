using System.Text;

namespace Wisedev.Magic.Titam.JSON;

public class LogicJSONBoolean : LogicJSONNode
{
    private readonly bool m_value;

    public LogicJSONBoolean(bool value)
    {
        m_value = value;
    }

    public bool IsTrue()
    {
        return m_value;
    }

    public override LogicJSONNodeType GetType()
    {
        return LogicJSONNodeType.BOOLEAN;
    }

    public override void WriteToString(StringBuilder builder)
    {
        builder.Append(m_value ? "true" : "false");
    }
}
