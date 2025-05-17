using Wisedev.Magic.Logic.Data;

namespace Wisedev.Magic.Logic.Util;

public static class LogicGamePlayUtil
{
    public static int TimeToXp(int t)
    {
        return (int)Math.Sqrt(t);
    }

    public static int GetSpeedUpCost(int time, int speedUpType)
    {
        int multiplier = 100;
        switch (speedUpType)
        {
            case 1:
                multiplier = LogicDataTables.GetGlobals().GetSpellSpeedUpCostMultiplier();
                break;
            case 2:
                multiplier = LogicDataTables.GetGlobals().GetHeroHealthSpeedUpCostMultiplier();
                break;
            case 3:
                multiplier = LogicDataTables.GetGlobals().GetTroopRequestSpeedUpCostMultiplier();
                break;
        }

        return LogicDataTables.GetGlobals().GetSpeedUpCost(time, multiplier);
    }

    public static int DPSToSingleHit(int dps, int ms)
    {
        long v2 = (27487790700L * dps * ms) >> 32;
        return (int)((v2 >> 6) + (v2 >> 31));
    }
}
