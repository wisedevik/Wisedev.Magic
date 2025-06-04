using Wisedev.Magic.Logic.Data.Core;
using Wisedev.Magic.Logic.Data.Objects;
using Wisedev.Magic.Logic.Data.Tables;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.Math;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicProjectile : LogicGameObject
{
    private LogicGameObject _target;
    private bool _pushBack;
    private bool _shouldDestruct;
    private LogicData _preferredTarget;
    private int _preferredTargetDamageMod;
    private int _damageRadius;
    private LogicEffectData _hitEffect;
    private LogicVector2 _targetPosition; // + 40
    private LogicVector2 _directionToTarget; // + 44
    private LogicVector2 _velocityVector; // + 48
    private LogicVector2 _targetOffset; // + 52
    private int _myTeam;
    private bool _flyingTarget;
    private int _randomHitRange;
    private bool _hasReachedTarget;
    private int _damage;

    public LogicProjectile(LogicData data, LogicLevel level) : base(data, level)
    {
        _targetPosition = new LogicVector2();
        _directionToTarget = new LogicVector2();
        _velocityVector = new LogicVector2();
        _targetOffset = new LogicVector2();
    }

    public LogicProjectileData GetProjectileData()
    {
        return (LogicProjectileData)this._data;
    }

    public override void RemoveGameObjectReferences(LogicGameObject gameObject)
    {
        if (_target == gameObject)
        {
            _target = null;
        }
    }

    public override void SubTick()
    {
        base.SubTick();
        if (!_hasReachedTarget)
        {
            if (_target != null && _target.GetMovementComponent() != null)
            {
                _targetPosition.Set(_target.GetMidX(), _target.GetMidY());
                _targetPosition.Add(_targetOffset);
            }

            _directionToTarget.Set(_targetPosition.GetX() - this.GetX(), _targetPosition.GetY() - this.GetY());

            int threshold = 16 * GetProjectileData().GetSpeed() / 1000;
            if (threshold * threshold >= _directionToTarget.GetDistanceSquaredTo(0, 0))
            {
                this.TargetReached();
            }
            else
            {
                _directionToTarget.Normalize(threshold);
                this.SetPositionXY(_directionToTarget.GetX() + this.GetX(), _directionToTarget.GetY() + this.GetY());
                _velocityVector.Set(_directionToTarget.GetX(), _directionToTarget.GetY());
            }
        }
    }

    public void TargetReached()
    {
        if (_target != null)
        {
            if (this.GetHitpointComponent() != null)
            {
                if (this._hitEffect != null)
                {
                    // Listener;
                }

                int damage = this._damage;
                if (LogicCombatComponent.IsPreferredTarget(this._preferredTarget, _target))
                    damage = this._damage * this._preferredTargetDamageMod / 100;

                if (damage <= -1)
                {
                    if (_target.GetData().GetDataType() == LogicDataType.HERO)
                    {
                        long tmp = (1374389535L * LogicDataTables.GetGlobals().GetHeroHealMultiplier() * damage) >> 32;
                        damage = (int)(tmp >> 5) + (int)(tmp >> 31);
                    }
                }

                if (_damageRadius < 1)
                {
                    // TODO!
                }
                else
                {
                    // TODO!
                }
            }
        }
        else
        {
            if (_damageRadius >= 1)
            {
                // TODO: LogicLevel::areaDamage()
            }
        }

        _hasReachedTarget = true;
    }

    public void SetTarget(int x, int y, int value)
    {

    }

    public void SetTargetPos(int x, int y, int team, bool flag)
    {
        _targetPosition.Set(x, y);
        _myTeam = team;
        _randomHitRange = flag ? 1000 : 0;
        _flyingTarget = flag;
    }

    public void SetMyTeam(int value)
    {
        _myTeam = value;
    }

    public int GetTargetX()
    {
        return _targetPosition.GetX();
    }

    public int GetTargetY()
    {
        return _targetPosition.GetY();
    }

    public long GetSpeed()
    {
        long tmp = (4398046512L * GetProjectileData().GetSpeed()) >> 32;
        return (tmp >> 6) + (tmp >> 31);
    }

    public override void SetInitialPosition(int x, int y)
    {
        _directionToTarget.Set(_targetPosition.GetX() - x, _targetPosition.GetY() - y);
        _directionToTarget.Normalize((GetProjectileData().GetStartOffset() << 9) / 100);
        base.SetInitialPosition(_directionToTarget.GetX() + x, _directionToTarget.GetY() + y);

        _velocityVector.Destruct();
        _velocityVector = null;
    }

    public void SetHitEffect(LogicEffectData effect)
    {
        _hitEffect = effect;
    }

    public void SetPushBack(bool pushBack)
    {
        _pushBack = pushBack;
    }

    public override int GetHeightInTiles()
    {
        return 0;
    }

    public override int GetWidthInTiles()
    {
        return 0;
    }

    public override int GetType()
    {
        return 2;
    }

    public void SetPreferredTargetDamageMod(LogicData data, int damage)
    {
        _preferredTarget = data;
        _preferredTargetDamageMod = damage;
    }

    public override bool ShouldDestruct()
    {
        return _shouldDestruct;
    }
}
