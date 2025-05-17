using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Logic.Util;
using Wisedev.Magic.Titan.JSON;

namespace Wisedev.Magic.Logic.GameObject.Component;

public class LogicHeroBaseComponent : LogicComponent
{
    private LogicHeroData m_data;
    private LogicTimer m_upgradingTimer;
    private int m_upgLevel;

    public LogicHeroBaseComponent(LogicGameObject gameObject, LogicHeroData data) : base(gameObject)
    {
        this.m_data = data;
    }

    public bool IsUpgrading()
    {
        return this.m_upgradingTimer != null;
    }

    public int GetRemainingUpgradeSeconds()
    {
        return this.m_upgradingTimer != null ? this.m_upgradingTimer.GetRemainingSeconds(this._gameObject.GetLevel().GetLogicTime()) : 0;
    }

    public int GetRemainingMS()
    {
        return this.m_upgradingTimer != null ? this.m_upgradingTimer.GetRemainingMS(this._gameObject.GetLevel().GetLogicTime()) : 0;
    }

    public bool SpeedUpUpgrade()
    {
        if (this.m_upgradingTimer != null)
        {
            int remainingSecs = this.m_upgradingTimer.GetRemainingSeconds(this._gameObject.GetLevel().GetLogicTime());
            int speedUpCost = LogicGamePlayUtil.GetSpeedUpCost(remainingSecs, 0);

            LogicAvatar homeOwnerAvatar = this._gameObject.GetLevel().GetHomeOwnerAvatar();

            if (homeOwnerAvatar.IsClientAvatar())
            {
                LogicClientAvatar clientAvatar = (LogicClientAvatar)homeOwnerAvatar;
                if (clientAvatar.HasEnoughDiamonds(speedUpCost, true, this._gameObject.GetLevel()))
                {
                    clientAvatar.UseDiamonds(speedUpCost);

                    this.FinishUpgrading(true);
                }
            }
            return true;
        }

        return false;
    }

    public void FinishUpgrading(bool tick)
    {
        if (this.m_upgradingTimer != null)
        {
            LogicAvatar homeOwnerAvatar = this._gameObject.GetLevel().GetHomeOwnerAvatar();

            if (homeOwnerAvatar.GetUnitUpgradeLevel(this.m_data) < this.m_upgLevel || this.m_upgLevel == 0)
            {
                homeOwnerAvatar.CommodityCountChangeHelper(1, this.m_data, 1);
            }

            _gameObject.GetLevel().GetWorkerManager().DeallocateWorker(this._gameObject);
        }
    }

    public void SetFullHealth()
    {
        LogicAvatar homeOwnerAvatar = this._gameObject.GetLevel().GetHomeOwnerAvatar();

        homeOwnerAvatar.SetHeroHealth(this.m_data, 0);
    }

    public override void Save(LogicJSONObject jsonObject)
    {
        if (this.m_upgradingTimer != null && this.m_data != null)
        {
            LogicJSONObject heroObj = new LogicJSONObject();
            heroObj.Put("level", new LogicJSONNumber(this.m_upgLevel));
            heroObj.Put("t", new LogicJSONNumber(this.m_upgradingTimer.GetRemainingSeconds(this._gameObject.GetLevel().GetLogicTime())));

            jsonObject.Put("hero_upg", heroObj);
        }
    }

    public override void Load(LogicJSONObject jsonObject)
    {
        if (this.m_upgradingTimer != null)
        {
            this.m_upgradingTimer.Destruct();
            this.m_upgradingTimer = null;
        }

        LogicJSONObject heroObj = jsonObject.GetJSONObject("hero_upg");

        if (heroObj != null)
        {
            LogicJSONNumber levelObject = heroObj.GetJSONNumber("level");
            LogicJSONNumber timerObject = heroObj.GetJSONNumber("t");

            if (levelObject != null)
            {
                this.m_upgLevel = levelObject.GetIntValue();
            }

            if (timerObject != null)
            {
                this.m_upgradingTimer = new LogicTimer();
                this.m_upgradingTimer.StartTimer(timerObject.GetIntValue(), this._gameObject.GetLevel().GetLogicTime());
                this._gameObject.GetLevel().GetWorkerManager().AllocateWorker(this._gameObject);
            }
        }
    }
}
