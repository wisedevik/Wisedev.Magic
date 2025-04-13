namespace Wisedev.Magic.Logic.Time;

public class LogicTimer
{
    private int _remainingTime;

    public int GetRemainingSeconds(LogicTime time)
    {
        int remaining = this._remainingTime - time.GetTick();

        if (remaining >= 1)
        {
            return Math.Max((remaining + 59) / 60, 1);
        }

        return 0;
    }

    public void Destruct()
    {
        ;
    }

    public void StartTimer(int sec, LogicTime time)
    {
        this._remainingTime = time.GetTick() + 60 * sec;
    }
}
