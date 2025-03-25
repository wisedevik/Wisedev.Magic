using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Titam.DataStream;
using Wisedev.Magic.Titam.Logic;

namespace Wisedev.Magic.Logic.Entries;

public class AllianceMemberEntry
{
    private LogicLong _avatarId;
    private string _facebookId;
    private string _name;
    private int _role;
    private int _expLevel;
    private LogicLeagueData _leagueTypeData;
    private int _score;
    private int _donations;
    private int _donationsReceived;
    private int _order;
    private int _previousOrder;
    private bool _newMember;
    private LogicLong _homeId;

    public AllianceMemberEntry()
    {
        this._avatarId = new LogicLong();
        this._facebookId = null;

    }

    public bool HasLowerRoleThan(int role)
    {
        if (role == 3)
        {
            if (this._role != 1)
                return false;
            return true;
        }
        else
        {
            if (role == 2)
            {
                return this._role != 2;
            }
        }

        return false;
    }

    public void Encode(ChecksumEncoder encoder)
    {
        encoder.WriteLong(this._avatarId);
        encoder.WriteString(this._facebookId);
        encoder.WriteString(this._name);
        encoder.WriteInt(this._role);
        encoder.WriteInt(this._expLevel);
        ByteStreamHelper.WriteDataReference(encoder, this._leagueTypeData);
        encoder.WriteInt(this._score);
        encoder.WriteInt(this._donations);
        encoder.WriteInt(this._donationsReceived);
        encoder.WriteInt(this._order);
        encoder.WriteInt(this._previousOrder);
        encoder.WriteBoolean(this._newMember);
        if (this._homeId != null)
        {
            encoder.WriteBoolean(true);
            encoder.WriteLong(this._homeId);
        }
        else
        {
            encoder.WriteBoolean(false);
        }
    }

    public void SetAvatarId(LogicLong value)
    {
        this._avatarId = value;
    }

    public void SetFacebookId(string value)
    {
        this._facebookId = value;
    }

    public void SetName(string value)
    {
        this._name = value;
    }

    public void SetRole(int value)
    {
        this._role = value;
    }

    public void SetExpLevel(int value)
    {
        this._expLevel = value;
    }

    public void SetLeagueType(LogicLeagueData value)
    {
        this._leagueTypeData = value;
    }

    public void SetScore(int value)
    {
        this._score = value;
    }

    public void SetDonations(int value)
    {
        this._donations = value;
    }

    public void SetDonationsReceived(int value)
    {
        this._donationsReceived = value;
    }

    public void SetOrder(int value)
    {
        this._order = value;
    }

    public void SetPreviousOrder(int value)
    {
        this._previousOrder = value;
    }

    public void SetNewMember(bool value)
    {
        this._newMember = value;
    }

    public void SetHomeId(LogicLong value)
    {
        this._homeId = value;
    }
}
