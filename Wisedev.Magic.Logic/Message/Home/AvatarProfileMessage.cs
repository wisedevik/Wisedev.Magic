using Wisedev.Magic.Logic.Entries;
using Wisedev.Magic.Titam.Message;

namespace Wisedev.Magic.Logic.Message.Home;

public class AvatarProfileMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 24334;

    private AvatarProfileFullEntry _avatarProfileFullEntry;

    public AvatarProfileMessage() : base(0)
    {
        this._avatarProfileFullEntry = null;
    }

    public override void Encode()
    {
        base.Encode();
        this._avatarProfileFullEntry.Encode(_stream);
    }

    public void SetAvatarProfileFullEntry(AvatarProfileFullEntry avatarProfileFullEntry)
    {
        this._avatarProfileFullEntry = avatarProfileFullEntry;
    }

    public override short GetMessageType()
    {
        return AvatarProfileMessage.MESSAGE_TYPE;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}
