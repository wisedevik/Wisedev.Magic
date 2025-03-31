namespace Wisedev.Magic.Logic.Util;

public static class LogicGamePlayUtil
{
    public static int TimeToXp(int t)
    {
        return (int)Math.Sqrt(t);
    }

    public static int DPSToSingleHit(int dps, int ms)
    {
        long v2 = (27487790700L * dps * ms) >> 32;
        return (int)((v2 >> 6) + (v2 >> 31));
    }
}
