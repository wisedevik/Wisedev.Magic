using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Data;

namespace Wisedev.Magic.Logic.Mode;

public class LogicGameListener
{
    public virtual void Destruct()
    {
        ;
    }

    public virtual void MapChanged()
    {
        ;
    }

    public virtual void NotEnoughDiamonds()
    {
        ;
    }

    public virtual void NotEnoughResources(LogicResourceData data, int cnt, LogicCommand command, bool callListener)
    {
        ;
    }

    public virtual void AchievementCompleted(LogicAchievementData data)
    {
        ;
    }

    public virtual void AchievementProgress(LogicAchievementData data)
    {
        ;
    } 
}
