using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Level;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicSpell : LogicGameObject
{
    private int _upgradeLevel;
    private int _hitCount;

    public LogicSpell(LogicData data, LogicLevel level) : base(data, level)
    {
    }

    public LogicSpellData GetSpellData()
    {
        return (LogicSpellData)_data;
    }

    public override void SetInitialPosition(int x, int y)
    {
        base.SetInitialPosition(x, y);
    }

    public override bool ShouldDestruct()
    {
        if (_level.IsInCombatState())
            return _hitCount >= GetSpellData().GetNumberOfHits(_upgradeLevel);

        return true;
    }

    public override int GetMidX()
    {
        return this.GetX();
    }

    public override int GetMidY()
    {
        return this.GetY();
    }

    public int Noise(int v)
    {
        return (1103515245 * v + 12345) >> 16;
    }

    public override bool IsStaticObject()
    {
        return false;
    }

    public override int GetType()
    {
        return 7;
    }
}
