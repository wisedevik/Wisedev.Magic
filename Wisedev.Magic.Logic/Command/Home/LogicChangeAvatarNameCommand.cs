using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Command.Home;

[LogicCommand(3)]
public class LogicChangeAvatarNameCommand : LogicServerCommand
{
    private string _name;

    public LogicChangeAvatarNameCommand()
    {
        this._name = string.Empty;
    }

    public override void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteString(this._name);
        base.Encode(encoder);
    }

    public override void Decode(ByteStream stream)
    {
        this._name = stream.ReadString();
        base.Decode(stream);
    }

    public override int Execute(LogicLevel level)
    {
        LogicClientAvatar playerAvatar = level.GetPlayerAvatar();
        if (playerAvatar != null)
        {
            playerAvatar.SetName(this._name);
            playerAvatar.SetNameSetByUser(true);
            return 0;
        }

        return -1;
    }

    public void SetName(string name)
    {
        this._name = name;
    }

    public override int GetCommandType()
    {
        return 3;
    }
}
