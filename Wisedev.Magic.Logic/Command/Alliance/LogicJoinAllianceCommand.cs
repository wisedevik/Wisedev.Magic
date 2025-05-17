using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.Logic;

namespace Wisedev.Magic.Logic.Command.Alliance;

public class LogicJoinAllianceCommand : LogicServerCommand
{
    public LogicLong AllianceId { get; set; }
    public string AllianceName { get; set; }
    public LogicAllianceBadgeData AllianceBadgeData { get; set; }
    public bool IsAllianceCreator { get; set; }

    public override void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteLong(AllianceId);
        encoder.WriteString(AllianceName);
        ByteStreamHelper.WriteDataReference(encoder, AllianceBadgeData);
        encoder.WriteBoolean(IsAllianceCreator);
        base.Encode(encoder);
    }

    public override int GetCommandType()
    {
        return 1;
    }
}
