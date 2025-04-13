using System.Text;

namespace Wisedev.Magic.Titan.JSON;

public abstract class LogicJSONNode
{
    public abstract LogicJSONNodeType GetType();
    public abstract void WriteToString(StringBuilder builder);
}

public enum LogicJSONNodeType
{
ARRAY = 1,
OBJECT,
NUMBER,
STRING,
BOOLEAN,
NULL
}

