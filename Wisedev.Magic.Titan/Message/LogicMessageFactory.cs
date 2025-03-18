namespace Wisedev.Magic.Titam.Message;

public abstract class LogicMessageFactory
{
    public LogicMessageFactory()
    {
        ;
    }

    public virtual void Destruct()
    {
        ;
    }

    public abstract PiranhaMessage? CreateMessageByType(int messageType);
}
