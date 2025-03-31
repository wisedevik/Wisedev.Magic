using Wisedev.Magic.Titam.JSON;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Coldown;

public class LogicCooldown
{
    private int _targetGlobalId;
    private int _cooldownSeconds;

    public LogicCooldown(int targetGlobalId, int cooldownSeconds)
    {
        this._targetGlobalId = targetGlobalId;
        this._cooldownSeconds = 15 * cooldownSeconds;
    }

    public LogicCooldown()
    {
        ;
    }

    public void Destruct()
    {
        this._targetGlobalId = 0;
        this._cooldownSeconds = 0;
    }

    public void Tick()
    {
        if (this._cooldownSeconds >= 1)
            --this._cooldownSeconds;
    }

    public void FastForwardTime(int time)
    {
        this._cooldownSeconds = -15 * time + this._cooldownSeconds;

        if (this._cooldownSeconds < 0)
            this._cooldownSeconds = 0;
    }

    public void Load(LogicJSONObject jSONObject)
    {
        LogicJSONNode cooldownNode = jSONObject.Get("cooldown");
        LogicJSONNode targetNode = jSONObject.Get("target");

        if (cooldownNode != null)
            this._cooldownSeconds = jSONObject.GetJSONNumber("cooldown").GetIntValue();
        else
            Debugger.Error("LogicCooldown.Load - Cooldown was not found!");

        if (targetNode != null)
            this._targetGlobalId = jSONObject.GetJSONNumber("target").GetIntValue();
        else
            Debugger.Error("LogicCooldown.Load - Target was not found!");
    }

    public void Save(LogicJSONObject jSONObject)
    {
        jSONObject.Put("cooldown", new LogicJSONNumber(this._cooldownSeconds));
        jSONObject.Put("target", new LogicJSONNumber(this._targetGlobalId));
    }

    public int GetCooldownSeconds()
    {
        return this._cooldownSeconds / 15;
    }

    public int GetTargetGlobalId()
    {
        return this._targetGlobalId;
    }
}
