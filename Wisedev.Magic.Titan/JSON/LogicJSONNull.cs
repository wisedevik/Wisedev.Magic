using System.Text;

namespace Wisedev.Magic.Titam.JSON;

public class LogicJSONNull : LogicJSONNode
{
    public override LogicJSONNodeType GetType()
    {
        return LogicJSONNodeType.NULL;
    }

    public override void WriteToString(StringBuilder builder)
    {
        builder.Append("null");
    }
}
