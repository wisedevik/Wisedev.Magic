using Wisedev.Magic.Titam.DataStream;
using Wisedev.Magic.Titam.Logic;

namespace Wisedev.Magic.Logic.Avatar;

public class LogicClientAvatar : LogicAvatar
{
    private LogicLong? _id;
    private LogicLong? _currentHomeId;
    private LogicLong? _allianceId;
    private LogicLong? _leagueInstanceId;
    private LogicLong? _lastLeagueInstanceId;

    private int[] _resources = { 3000001, 3000002, 3000003 };
    public int[] TutorialSteps = Enumerable.Range(21000000, 13).ToArray();

    private string? _name;
    private string? _facebookId;
    private int _expLevel;
    private int _expPoints;
    private int _diamonds;
    private int _freeDiamonds;
    private int _attackKFactor;
    private int _attackRating;
    private int _score;
    private int _attackWinCount;
    private int _attackLoseCount;
    private int _defenseWinCount;
    private int _defenseLoseCount;
    private bool _nameSetByUser;
    private int _cumulativePurchasedDiamonds;

    public LogicLong? Id { get { return this._id; } private set { this._id = value; } }
    public LogicLong? CurrentHomeId { get { return this._currentHomeId; } private set { this._currentHomeId = value; } }
    public LogicLong? AllianceId { get { return this._allianceId; } private set { this._allianceId = value; } }
    public LogicLong? LeagueInstanceId { get { return this._leagueInstanceId; } private set { this._leagueInstanceId = value; } }
    public LogicLong? LastLeagueInstanceId { get { return this._lastLeagueInstanceId; } private set { this._lastLeagueInstanceId = value; } }
    public string? Name { get { return this._name; } private set { this._name = value; } }
    public string? FacebookId { get { return this._facebookId; } private set { this._facebookId = value; } }
    public int ExpLevel { get { return this._expLevel; } private set { this._expLevel = value; } }
    public int ExpPoints { get { return this._expPoints; } private set { this._expPoints = value; } }
    public int Diamonds { get { return this._diamonds; } private set { this._diamonds = value; } }
    public int FreeDiamonds { get { return this._freeDiamonds; } private set { this._freeDiamonds = value; } }
    public int AttackRating { get { return this._attackRating; } private set { this._attackRating = value; } }
    public int AttackKFactor { get { return this._attackKFactor; } private set { this._attackKFactor = value; } }
    public int Score { get { return this._score; } private set { this._score = value; } }
    public int AttackWinCount { get { return this._attackWinCount; } private set { this._attackWinCount = value; } }
    public int AttackLoseCount { get { return this._attackLoseCount; } private set { this._attackLoseCount = value; } }
    public int DefenseWinCount { get { return this._defenseWinCount; } private set { this._defenseWinCount = value; } }
    public int DefenseLoseCount { get { return this._defenseLoseCount; } private set { this._defenseLoseCount = value; } }
    public bool NameSetByUser { get { return this._nameSetByUser; } private set { this._nameSetByUser = value; } }
    public int CumulativePurchasedDiamonds { get { return this._cumulativePurchasedDiamonds; } private set { this._cumulativePurchasedDiamonds = value; } }

    public void SetId(LogicLong id)
    {
        this._id = id;
    }

    public void SetCurrentHomeId(LogicLong id)
    {
        this._currentHomeId = id;
    }

    public void SetAllianceId(LogicLong id)
    {
        this._allianceId = id;
    }

    public void SetLeagueInstanceId(LogicLong id)
    {
        this._leagueInstanceId = id;
    }

    public void SetLastLeagueInstanceId(LogicLong id)
    {
        this._lastLeagueInstanceId = id;
    }

    public void SetName(string name)
    {
        this._name = name;
    }

    public void SetFacebookId(string id)
    {
        this._facebookId = id;
    }

    public void SetExpLevel(int level)
    {
        this._expLevel = level;
    }

    public void SetExpPoints(int points)
    {
        this._expPoints = points;
    }

    public void SetDiamonds(int diamonds)
    {
        this._diamonds = diamonds;
    }

    public void SetFreeDiamonds(int diamonds)
    {
        this._freeDiamonds = diamonds;
    }

    public void SetAttackRating(int rating)
    {
        this._attackRating = rating;
    }

    public void SetAttackKFactor(int factor)
    {
        this._attackKFactor = factor;
    }

    public void SetScore(int score)
    {
        this._score = score;
    }

    public void SetAttackWinCount(int count)
    {
        this._attackWinCount = count;
    }

    public void SetAttackLoseCount(int count)
    {
        this._attackLoseCount = count;
    }

    public void SetDefenseWinCount(int count)
    {
        this._defenseWinCount = count;
    }

    public void SetDefenseLoseCount(int count)
    {
        this._defenseLoseCount = count;
    }

    public void SetNameSetByUser(bool set)
    {
        this._nameSetByUser = set;
    }

    public void SetCumulativePurchasedDiamonds(int diamonds)
    {
        this._cumulativePurchasedDiamonds = diamonds;
    }

    public override void Encode(ChecksumEncoder encoder)
    {
        base.Encode(encoder);
        encoder.WriteLong(this._id!);
        encoder.WriteLong(this._currentHomeId!);
        if (this._allianceId! != 0)
        {
            encoder.WriteBoolean(true);
            // TODO: Implement this
        }
        else
        {
            encoder.WriteBoolean(false);
        }

        if (this._leagueInstanceId! != 0)
        {
            encoder.WriteBoolean(true);
            // TODO: Implement this
        }
        else
        {
            encoder.WriteBoolean(false);
        }

        if (this._lastLeagueInstanceId! != 0)
        {
            encoder.WriteBoolean(true);
            //TODO: Implement this
        }
        else
        {
            encoder.WriteBoolean(false);
        }

        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteString(this._name);
        encoder.WriteString(this._facebookId);
        encoder.WriteInt(this._expLevel);
        encoder.WriteInt(this._expPoints);
        encoder.WriteInt(this._diamonds);
        encoder.WriteInt(this._freeDiamonds);
        encoder.WriteInt(this._attackRating);
        encoder.WriteInt(this._attackKFactor);
        encoder.WriteInt(this._score);
        encoder.WriteInt(this._attackWinCount);
        encoder.WriteInt(this._attackLoseCount);
        encoder.WriteInt(this._defenseWinCount);
        encoder.WriteInt(this._defenseLoseCount);
        encoder.WriteBoolean(this._nameSetByUser);
        encoder.WriteInt(this._cumulativePurchasedDiamonds);


        encoder.WriteInt(0);
        encoder.WriteInt(_resources.Length);
        foreach (var item in _resources)
        {
            encoder.WriteInt((int)item);
            encoder.WriteInt(0);
        }

        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);

        encoder.WriteInt(TutorialSteps.Length);
        foreach (var item in TutorialSteps)
        {
            encoder.WriteInt(item);
        }

        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
        encoder.WriteInt(0);
    }
}
