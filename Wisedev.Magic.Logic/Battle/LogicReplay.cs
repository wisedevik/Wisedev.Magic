using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titam.JSON;

namespace Wisedev.Magic.Logic.Battle;

public class LogicReplay
{
    private LogicLevel _level;
    private LogicJSONObject _jsonObject;
    private LogicJSONNumber _endTickNumber;

    public LogicReplay(LogicLevel level)
    {
        this._level = level;
        this._jsonObject = null;
        this._endTickNumber = null;
        this.RecordStart();
    }

    public void RecordStart()
    {
        this._jsonObject = new LogicJSONObject();
        this._endTickNumber = new LogicJSONNumber();

        LogicJSONObject levelJsonObj = new LogicJSONObject();
        LogicJSONObject visitorAvatarJsonObj = new LogicJSONObject();
        LogicJSONObject homeOwnerJsonObj = new LogicJSONObject();

        this._level.SaveToJSON(levelJsonObj);
        this._level.GetVisitorAvatar().SaveToReplay(visitorAvatarJsonObj);
        this._level.GetHomeOwnerAvatar().SaveToReplay(homeOwnerJsonObj);

        this._jsonObject.Put("level", levelJsonObj);
        this._jsonObject.Put("attacker", visitorAvatarJsonObj);
        this._jsonObject.Put("defender", homeOwnerJsonObj);
        this._jsonObject.Put("end_tick", this._endTickNumber);
        this._jsonObject.Put("cmd", new LogicJSONArray());
    }

    public void SubTick()
    {
        this._endTickNumber.SetIntValue(this._level.GetLogicTime().GetTick() + 1);
    }

    public void RecordCommand(LogicCommand command)
    {
        LogicJSONArray logicJSONArray = this._jsonObject.GetJSONArray("cmd");
        LogicJSONObject comandObject = new LogicJSONObject();
        LogicCommandManager.SaveCommandToJSON(comandObject, command);

        logicJSONArray.Add(comandObject);
    }

    public LogicJSONObject GetJSON()
    {
        return this._jsonObject;
    }
}
