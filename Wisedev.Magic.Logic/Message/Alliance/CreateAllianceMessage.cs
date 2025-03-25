using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Titam.Message;

namespace Wisedev.Magic.Logic.Message.Alliance;

[PiranhaMessage(CreateAllianceMessage.MESSAGE_TYPE)]
public class CreateAllianceMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 14301;

    private LogicAllianceBadgeData? _allianceBadgeData;
    private string _allianceName;
    private string _allianceDescription;
    private int _allianceType;
    private int _requiredScore;

    public CreateAllianceMessage() : base(0)
    {
    }

    public override void Decode()
    {
        base.Decode();
        this._allianceName = this._stream.ReadString();
        this._allianceDescription = this._stream.ReadString();
        this._allianceBadgeData = (LogicAllianceBadgeData?)ByteStreamHelper.ReadDataReference(_stream);
        this._allianceType = this._stream.ReadInt();
        this._requiredScore = this._stream.ReadInt();
    }

    public string GetAllianceName()
    {
        return this._allianceName;
    }

    public string GetAllianceDescription()
    {
        return this._allianceDescription;
    }

    public LogicAllianceBadgeData? GetAllianceBadgeData()
    {
        return this._allianceBadgeData;
    }

    public int GetAllianceType()
    {
        return this._allianceType;
    }

    public int GetRequiredScore()
    {
        return this._requiredScore;
    }

    public override short GetMessageType()
    {
        return CreateAllianceMessage.MESSAGE_TYPE;
    }
}
