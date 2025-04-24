using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.Logic;

namespace Wisedev.Magic.Logic.Command.Home;

public class LogicJoinAllianceCommand : LogicServerCommand
{
    private LogicLong _allianceId;
    private string _allianceName;
    private int _allianceBadgeId;
    private bool _allianceCreate;

    public override void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteLong(_allianceId);
        encoder.WriteString(_allianceName);
        encoder.WriteInt(_allianceBadgeId);
        encoder.WriteBoolean(_allianceCreate);
        base.Encode(encoder);
    }

    public void SetAllianceId(LogicLong allianceId)
    {
        _allianceId = allianceId;
    }

    public void SetAllianceName(string allianceName)
    {
        _allianceName = allianceName;
    }

    public void SetAllianceBadgeId(int allianceBadgeId)
    {
        _allianceBadgeId = allianceBadgeId;
    }

    public void SetAllianceCreate(bool allianceCreate)
    {
        _allianceCreate = allianceCreate;
    }

    public override int GetCommandType()
    {
        return 1;
    }
}
