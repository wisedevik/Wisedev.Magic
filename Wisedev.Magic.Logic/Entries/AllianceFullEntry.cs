using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Entries;

public class AllianceFullEntry
{
    private AllianceHeaderEntry _allianceHeaderEntry;
    private List<AllianceMemberEntry> _allianceMembers;
    private string _allianceDescription;

    public void Encode(ChecksumEncoder encoder)
    {
        this._allianceHeaderEntry.Encode(encoder);

        encoder.WriteString(this._allianceDescription);
        if (this._allianceMembers != null)
        {
            encoder.WriteInt(this._allianceMembers.Count);
            for (int i = 0; i < this._allianceMembers.Count; i++)
            {
                this._allianceMembers[i].Encode(encoder);
            }
        }
        else
        {
            encoder.WriteInt(-1);
        }
    }

    public void SetAllianceHeaderEntry(AllianceHeaderEntry entry)
    {
        this._allianceHeaderEntry = entry;
    }

    public void SetAllianceMembers(List<AllianceMemberEntry> members)
    {
        this._allianceMembers = members;
    }

    public void SetAllianceDescription(string description)
    {
        this._allianceDescription = description;
    }
}
