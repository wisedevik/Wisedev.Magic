using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.Utils;

namespace Wisedev.Magic.Logic.GameObject.Component;

public class LogicComponentManager
{
    private LogicLevel _level;
    private readonly List<LogicComponent>[] _components;

    public LogicComponentManager(LogicLevel level)
    {
        this._level = level;
        this._components = new List<LogicComponent>[LogicComponent.COMPONENT_TYPE_COUNT];

        for (int i = 0; i < LogicComponent.COMPONENT_TYPE_COUNT; i++)
        {
            this._components[i] = new List<LogicComponent>(32);
        }
    }

    public void Tick()
    {
        bool isInCombatState = this._level.IsInCombatState();

        for (int i = 0; i < LogicComponent.COMPONENT_TYPE_COUNT; ++i)
        {
            for (int k = 0, size = this._components[i].Count; k < size; ++k)
            {
                LogicComponent component = this._components[i][k];
                if (component.IsEnabled())
                    component.Tick();
            }

            if (i == 0 && !isInCombatState)
                i = 1;
        }
    }

    public void SubTick()
    {
        ;
    }

    public void AddComponent(LogicComponent component)
    {
        this._components[component.GetComponentType()].Add(component);
    }


    public void RemoveComponent(LogicComponent component)
    {
        List<LogicComponent> components = this._components[component.GetComponentType()];

        for (int i = 0, j = components.Count; i < j; i++)
        {
            if (components[i] == component)
            {
                components.RemoveAt(i);
                break;
            }
        }
    }
}
