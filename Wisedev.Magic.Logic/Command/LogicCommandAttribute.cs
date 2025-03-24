namespace Wisedev.Magic.Logic.Command;

[AttributeUsage(AttributeTargets.Class)]
public class LogicCommandAttribute(int t) : Attribute
{
    public int CommandType { get; } = t;
}
