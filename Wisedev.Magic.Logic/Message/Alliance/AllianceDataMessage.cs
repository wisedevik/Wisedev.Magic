using Wisedev.Magic.Logic.Entries;
using Wisedev.Magic.Titam.Message;

namespace Wisedev.Magic.Logic.Message.Alliance;

public class AllianceDataMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 24301;

    private AllianceFullEntry? _allianceFullEntry;

    public AllianceDataMessage() : base(0)
    {
        _allianceFullEntry = null;
    }

    public override void Encode()
    {
        base.Encode();
        this._allianceFullEntry.Encode(this._stream);
    }

    public void SetAllianceFullEntry(AllianceFullEntry allianceFullEntry)
    {
        this._allianceFullEntry = allianceFullEntry;
    }

    public override short GetMessageType()
    {
        return AllianceDataMessage.MESSAGE_TYPE;
    }
}
