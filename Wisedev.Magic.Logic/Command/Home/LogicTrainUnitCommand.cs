using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(COMMAND_TYPE)]
public class LogicTrainUnitCommand : LogicCommand
{
    public const int COMMAND_TYPE = 508;

    private int _buildingId;
    private int _unitType;
    private LogicCombatItemData _unitData;
    private int _trainCount;

    public override void Decode(ByteStream stream)
    {
        _buildingId = stream.ReadInt();
        _unitType = stream.ReadInt();
        _unitData = (LogicCombatItemData)ByteStreamHelper.ReadDataReference(stream, _unitType != 0 ? LogicDataType.SPELL : LogicDataType.CHARACTER);
        _trainCount = stream.ReadInt();
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

                    if (_trainCount < 101)
                    {
                        if (_trainCount >= 1)
                        {
                            if (_unitData.IsProductionEnabled())
                            {
                                LogicClientAvatar playerAvatar = level.GetPlayerAvatar();
                                int unitUpgradeLevel = playerAvatar.GetUnitUpgradeLevel(_unitData);

                                for (int i = 0; i < _trainCount; i++)
                                {
                                    // TOOD: в пизду кароче патом
                                }
                            }
                        }
                    }
                    else
                        Debugger.Error("LogicTraingUnitCommand - Count is too high");

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
