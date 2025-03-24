using Wisedev.Magic.Logic.Home.Listener;
using Wisedev.Magic.Titam.DataStream;
using Wisedev.Magic.Titam.Logic;

namespace Wisedev.Magic.Logic.Home;

public class LogicClientHome : LogicBase
{
    private LogicHomeChangeListener _listener;
    private LogicLong _id;
    private string? _homeJSON = null;
    private int _shieldDurationSeconds;
    private int _defenseRating;
    private int _defenseKFactor;

    public LogicLong Id { get { return this._id; } private set { this._id = value; } }
    public string HomeJSON { get { return this._homeJSON; } private set { this._homeJSON = value; } }
    public int ShieldDurationSeconds { get { return this._shieldDurationSeconds; } private set { this._shieldDurationSeconds = value; } }
    public int DefenseRating { get { return this._defenseRating; } private set { this._defenseRating = value; } }
    public int DefenseKFactor { get { return this._defenseKFactor; } private set { this._defenseKFactor = value; } }

    public LogicClientHome()
    {
        this._listener = new LogicHomeChangeListener();
        this._id = new LogicLong();
    }

    public override void Encode(ChecksumEncoder encoder)
    {
        base.Encode(encoder);
        encoder.WriteLong(_id);
        encoder.WriteString(_homeJSON);
        encoder.WriteInt(_shieldDurationSeconds);
        encoder.WriteInt(_defenseRating);
        encoder.WriteInt(_defenseKFactor);
    }

    public void SetHomeChangeListener(LogicHomeChangeListener listener)
    {
        this._listener = listener;
    }

    public LogicHomeChangeListener GetHomeChangeListener()
    {
        return this._listener;
    }

    public void SetId(LogicLong id)
    {
        this._id = id;
    }

    public LogicLong GetId()
    {
        return this._id;
    }

    public void SetHomeJSON(string json)
    {
        this._homeJSON = json;
    }

    public string GetHomeJSON()
    {
        return this._homeJSON;
    }

    public void SetShieldDurationSeconds(int seconds)
    {
        this._shieldDurationSeconds = seconds;
    }

    public int GetShieldDurationSeconds()
    {
        return this._shieldDurationSeconds;
    }

    public void SetDefenseRating(int rating)
    {
        this._defenseRating = rating;
    }

    public int GetDefenseRating()
    {
        return this._defenseRating;
    }

    public void SetDefenseKFactor(int kFactor)
    {
        this._defenseKFactor = kFactor;
    }

    public int GetDefenseKFactor()
    {
        return this._defenseKFactor;
    }
}
