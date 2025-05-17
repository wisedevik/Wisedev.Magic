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
        this._remainingTime = 0;
    }

    public void StartTimer(int sec, LogicTime time)
    {
        this._remainingTime = time.GetTick() + 60 * sec;
    }

    public int GetRemainingMS(LogicTime time)
    {
        int tickDiff = this._remainingTime - time.GetTick();

        int ms = 1000 * (tickDiff / 60);
        int remainder= tickDiff % 60;
        if (remainder >= 1)
            ms += (2133 * remainder) >> 7;

        return ms;
    }
}
