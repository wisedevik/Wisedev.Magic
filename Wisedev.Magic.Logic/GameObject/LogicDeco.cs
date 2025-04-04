using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Level;

namespace Wisedev.Magic.Logic.GameObject;

public class LogicDeco : LogicGameObject
{
    public LogicDeco(LogicDecoData data, LogicLevel level) : base(data, level)
    {
    }

    public LogicDecoData GetDecoData()
    {
        return (LogicDecoData)this._data;
    }

    public override int GetGameObjectType()
    {
        return 6;
    }

}
