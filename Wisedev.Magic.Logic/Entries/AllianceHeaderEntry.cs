using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.Logic;

namespace Wisedev.Magic.Logic.Entries;

public class AllianceHeaderEntry
{
    private LogicLong _allianceId;
    private string _allianceName;
    private LogicAllianceBadgeData? _badgeData;
    private int _allianceType;
    private int _numberOfMembers;
    public int _score;
    private int _requiredScore;

    public AllianceHeaderEntry()
    {
        _allianceId = new LogicLong();
        _allianceName = string.Empty;
        _badgeData = null;
        _allianceType = 0;
        _score = 0;
        _requiredScore = 0;
    }

    public void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteLong(this._allianceId);
        encoder.WriteString(this._allianceName);
        ByteStreamHelper.WriteDataReference(encoder, this._badgeData);
        encoder.WriteInt(this._allianceType);
        encoder.WriteInt(this._numberOfMembers);
        encoder.WriteInt(this._score);
        encoder.WriteInt(this._requiredScore);
    }

    public void SetAllianceId(LogicLong allianceId)
    {
        this._allianceId = allianceId;
    }

    public void SetAllianceName(string allianceName)
    {
        this._allianceName = allianceName;
    }

    public void SetAllianceBadgeData(LogicAllianceBadgeData badgeData)
    {
        this._badgeData = badgeData;
    }

    public void SetAllianceType(int allianceType)
    {
        this._allianceType = allianceType;
    }

    public void SetNumberOfMembers(int numberOfMembers)
    {
        this._numberOfMembers = numberOfMembers;
    }

    public void SetScore(int score)
    {
        this._score = score;
    }

    public void SetRequiredScore(int requiredScore)
    {
        this._requiredScore = requiredScore;
    }

}
