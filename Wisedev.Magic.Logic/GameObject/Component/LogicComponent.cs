namespace Wisedev.Magic.Logic.GameObject.Component;

public class LogicComponent
{
    public virtual void SubTick()
    {
        ;
    }

    public virtual void LoadingFinished()
    {
        ;
    }

    public virtual int GetChecksum()
    {
        return 0;
    }

    public virtual void FastForwardTime(int t)
    {
        ;
    }


}
