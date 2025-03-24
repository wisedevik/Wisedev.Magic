namespace Wisedev.Magic.Logic.Time;

public class LogicTime
{
    private int _tick;
    private int _fullTick;

    public bool IsFullTick()
    {
        return ((this._tick + 1) & 3) == 0;
    }

    public void IncreaseSubTick()
    {
        ++this._tick;

        if ((this._tick & 3) == 0)
        {
            ++this._fullTick;
        }
    }

    public int GetTick()
    {
        return this._tick;
    }

    public int GetFullTick()
    {
        return this._fullTick;
    }
}
