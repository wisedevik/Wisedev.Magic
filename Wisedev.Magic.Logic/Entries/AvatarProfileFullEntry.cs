using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Entries;

public class AvatarProfileFullEntry
{
    private LogicClientAvatar _logicClientAvatar;
    private int _donations;
    private int _donationsReceived;

    public AvatarProfileFullEntry()
    {
        this._logicClientAvatar = null;
        this._donations = 0;
        this._donationsReceived = 0;
    }

    public void SetLogicClientAvatar(LogicClientAvatar logicClientAvatar)
    {
        this._logicClientAvatar = logicClientAvatar;
    }

    public LogicClientAvatar GetLogicClientAvatar()
    {
        return this._logicClientAvatar;
    }

    public void SetDonations(int donations)
    {
        this._donations = donations;
    }

    public int GetDonations()
    {
        return this._donations;
    }

    public void SetDonationsReceived(int donationsReceived)
    {
        this._donationsReceived = donationsReceived;
    }

    public int GetDonationsReceived()
    {
        return this._donationsReceived;
    }

    public void Encode(ChecksumEncoder encoder)
    {
        this._logicClientAvatar.Encode(encoder);
        encoder.WriteInt(this._donations);
        encoder.WriteInt(this._donationsReceived);
    }
}
