namespace Wisedev.Magic.Logic.Message;

[AttributeUsage(AttributeTargets.Class)]
class PiranhaMessageAttribute(int type) : Attribute
{
    public int MessageType { get; } = type;
}
