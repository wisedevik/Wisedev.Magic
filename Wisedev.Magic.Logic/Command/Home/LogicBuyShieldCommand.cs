using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(522)]
public class LogicBuyShieldCommand : LogicCommand
{
    private LogicShieldData _shieldData;

    public override void Decode(ByteStream stream)
    {
        this._shieldData = (LogicShieldData)ByteStreamHelper.ReadDataReference(stream, LogicDataType.SHIELD);
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        if (this._shieldData != null)
        {

        }

        return -1;
    }

    public override int GetCommandType()
    {
        return 522;
    }
}
