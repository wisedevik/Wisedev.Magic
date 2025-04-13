using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(532)]
public class LogicNewShopItemsSeenCommand : LogicCommand
{
    public const int COMMAND_TYPE = 532;

    private int _newShopItemsType;
    private int _newShopItemsIdx;
    private int _newShopItemsCount;

    public override void Decode(ByteStream stream)
    {
        this._newShopItemsIdx = stream.ReadInt();
        this._newShopItemsType = stream.ReadInt();
        this._newShopItemsCount = stream.ReadInt();
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        Debugger.Print($"LogicNewShopItemsSeenCommand.Execute: m_newShopItemsType={this._newShopItemsType} m_newShopItemsIdx={this._newShopItemsIdx} m_newShopItemsCount={this._newShopItemsCount}");

        if (this._newShopItemsType == 0 ||
            this._newShopItemsType == 11 ||
            this._newShopItemsType == 17)
        {
            if (level.SetUnlockedShopItemCount(this._newShopItemsType, this._newShopItemsIdx, this._newShopItemsCount))
            {
                return 0;
            }

            return -2;
        }

        return -1;
    }

    public override int GetCommandType()
    {
        return LogicNewShopItemsSeenCommand.COMMAND_TYPE;
    }
}
