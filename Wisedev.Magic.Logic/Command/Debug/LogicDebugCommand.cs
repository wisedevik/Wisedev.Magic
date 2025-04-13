using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Command.Debug;

public class LogicDebugCommand : LogicCommand
{
    private int _debugAction;

    public LogicDebugCommand() : base()
    {
        ;
    }

    public LogicDebugCommand(int action) : base()
    {
        this._debugAction = action;
    }

    public void SetDebugAction(int a)
    {
        this._debugAction = a;
    }

    public override void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteInt(this._debugAction);
        base.Encode(encoder);
    }

    public override int GetCommandType()
    {
        return 1000;
    }

    public override int Execute(LogicLevel level)
    {
        LogicClientAvatar playerAvatar = level.GetPlayerAvatar();

        switch (this._debugAction)
        {
            case 3:
                int diamondCount = playerAvatar.GetDiamonds();

                if (diamondCount < 50000)
                {
                    playerAvatar.SetDiamonds(diamondCount);
                    playerAvatar.GetChangeListener().FreeDiamondsAdded(50000 - diamondCount);
                }

                break;
        }

        return 0;
    }
}
