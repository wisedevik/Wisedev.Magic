using System.Text.Json.Nodes;
using Wisedev.Magic.Titam.JSON;

namespace Wisedev.Magic.Logic.Coldown;

public class LogicCooldownManager
{
    private List<LogicCooldown> _cooldowns;

    public LogicCooldownManager()
    {
        this._cooldowns = new List<LogicCooldown>();
    }

    public void DeleteCooldowns()
    {
        for (int i = 0; i < this._cooldowns.Count; i++)
        {
            this._cooldowns[i].Destruct();
            this._cooldowns[i] = null;
        }
    }

    public void Tick() 
    {
        for (int i = 0; i < this._cooldowns.Count; ++i)
        {
            this._cooldowns[i].Tick();

            if (this._cooldowns[i].GetCooldownSeconds() <= 0)
                this._cooldowns.RemoveAt(i);
        }
    }

    public void FastForwardTime(int time)
    {
        for (int i = 0;i < this._cooldowns.Count; i++)
            this._cooldowns[i].FastForwardTime(time);
    }

    public void AddCooldown(int targetGloablId, int cooldownSeconds)
    {
        this._cooldowns.Add(new LogicCooldown(targetGloablId, cooldownSeconds));
    }

    public int GetCooldownSeconds(int id)
    {
        for (int i = 0; i < _cooldowns.Count; ++i)
        {
            if (this._cooldowns[i].GetTargetGlobalId() == id)
            {
                return this._cooldowns[i].GetCooldownSeconds();
            }
        }

        return 0;
    }

    public void Save(LogicJSONObject jSONObject)
    {
        LogicJSONArray jsonArray = new LogicJSONArray();

        for (int i = 0; i < this._cooldowns.Count; ++i)
        {
            LogicJSONObject cooldownObj = new LogicJSONObject();
            this._cooldowns[i].Save(cooldownObj);
            jsonArray.Add(cooldownObj);
        }

        jSONObject.Put("cooldowns", jsonArray);
    }

    public void Load(LogicJSONObject jSONObject)
    {
        LogicJSONArray cooldownArray = jSONObject.GetJSONArray("cooldowns");

        if (cooldownArray != null)
        {
            int size = cooldownArray.Size();

            for (int i = 0; i < size; i++)
            {
                LogicCooldown cooldown = new LogicCooldown();
                cooldown.Load(cooldownArray.GetJSONObject(i));
                this._cooldowns.Add(cooldown);
            }
        }
    }
}
