using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titam.DataStream;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(512)]
public class LogicBuyDecoCommand : LogicCommand
{
    private int _x;
    private int _y;
    private LogicDecoData _decoData;

    public override void Decode(ByteStream stream)
    {
        this._x = stream.ReadInt();
        this._y = stream.ReadInt();
        this._decoData = (LogicDecoData)ByteStreamHelper.ReadDataReference(stream, LogicDataType.DECO);
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        LogicDeco? deco = (LogicDeco)LogicGameObjectFactory.CreateGameObject(this._decoData, level);
        if (deco != null)
        {
            deco.SetInitialPosition(this._x << 9, this._y << 9);
            level.GetGameObjectManager().AddGameObject(deco, -1);
        }

        return base.Execute(level);
    }

    public override int GetCommandType()
    {
        return 512;
    }
}
