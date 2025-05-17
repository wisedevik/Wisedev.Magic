using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(COMMAND_TYPE)]
public class LogicSpeedUpConstructionCommand : LogicCommand
{
    public const int COMMAND_TYPE = 504;

    private int _buildingId;

    public override void Decode(ByteStream stream)
    {
        _buildingId = stream.ReadInt();
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        LogicGameObject gameObject = level.GetGameObjectManager().GetGameObjectByID(_buildingId);
        if (gameObject != null)
        {
            switch (gameObject.GetType())
            {
                case 0:
                    LogicBuilding building = (LogicBuilding)gameObject;
                    building.SpeedUpConstruction();
                    break;
            }
        }

        return -1;
    }

    public override int GetCommandType()
    {
        return COMMAND_TYPE;
    }
}
