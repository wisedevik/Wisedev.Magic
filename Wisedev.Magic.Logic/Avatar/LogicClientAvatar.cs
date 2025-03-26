using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Helper;
using Wisedev.Magic.Logic.Level;
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
    private int _leagueType;

    public LogicLong? Id { get { return this._id; } set { this._id = value; } }
    public LogicLong? CurrentHomeId { get { return this._currentHomeId; } set { this._currentHomeId = value; } }
    public LogicLong? AllianceId { get { return this._allianceId; } set { this._allianceId = value; } }
    public LogicLong? LeagueInstanceId { get { return this._leagueInstanceId; } set { this._leagueInstanceId = value; } }
    public LogicLong? LastLeagueInstanceId { get { return this._lastLeagueInstanceId; } set { this._lastLeagueInstanceId = value; } }
    public string? Name { get { return this._name; } set { this._name = value; } }
    public string? FacebookId { get { return this._facebookId; } set { this._facebookId = value; } }
    public int ExpLevel { get { return this._expLevel; } set { this._expLevel = value; } }
    public int ExpPoints { get { return this._expPoints; } set { this._expPoints = value; } }
    public int Diamonds { get { return this._diamonds; } set { this._diamonds = value; } }
    public int FreeDiamonds { get { return this._freeDiamonds; } set { this._freeDiamonds = value; } }
    public int AttackRating { get { return this._attackRating; } set { this._attackRating = value; } }
    public int AttackKFactor { get { return this._attackKFactor; } set { this._attackKFactor = value; } }
    public int Score { get { return this._score; } set { this._score = value; } }
    public int AttackWinCount { get { return this._attackWinCount; } set { this._attackWinCount = value; } }
    public int AttackLoseCount { get { return this._attackLoseCount; } set { this._attackLoseCount = value; } }
    public int DefenseWinCount { get { return this._defenseWinCount; } set { this._defenseWinCount = value; } }
    public int DefenseLoseCount { get { return this._defenseLoseCount; } set { this._defenseLoseCount = value; } }
    public bool NameSetByUser { get { return this._nameSetByUser; } set { this._nameSetByUser = value; } }
    public int CumulativePurchasedDiamonds { get { return this._cumulativePurchasedDiamonds; } set { this._cumulativePurchasedDiamonds = value; } }
    public int LeagueType { get { return this._leagueType; } set { this._leagueType = value; } }

    public static LogicClientAvatar GetDefaultAvatar()
    {
        LogicClientAvatar avatar = new LogicClientAvatar();
        LogicGlobals globalsInstance = LogicDataTables.GetGlobals();

        avatar._diamonds = globalsInstance.GetStartingDiamonds();
        avatar._freeDiamonds = globalsInstance.GetStartingDiamonds();

        avatar.SetResourceCount(LogicDataTables.GetGoldData(), globalsInstance.GetStartingGold());
        avatar.SetResourceCount(LogicDataTables.GetElexirData(), globalsInstance.GetStartingElexir());

        for (int i = 0; i < 13; i++)
        {
            LogicMissionData missionData = (LogicMissionData)LogicDataTables.GetDataById(21000000 + i);
            if (missionData != null)
                avatar.SetMissionCompleted(missionData, true);
        }

        return avatar;
    }

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

        encoder.WriteInt(this._leagueType);
        encoder.WriteInt(this._allianceCastleLevel);
        encoder.WriteInt(this._allianceCastleTotalCapacity);
        encoder.WriteInt(this._allianceCastleUsedCapacity);
        encoder.WriteInt(this._townHallLevel);
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


        encoder.WriteInt(this._resourceCap.Count);
        for (int i = 0; i < this._resourceCap.Count; i++)
        {
            this._resourceCap[i].Encode(encoder);
        }

        encoder.WriteInt(this._resourceCount.Count);
        for (int i = 0; i < this._resourceCount.Count; i++)
        {
            this._resourceCount[i].Encode(encoder);
        }

        encoder.WriteInt(this._unitCount.Count);
        for (int i = 0; i < this._unitCount.Count; i++)
        {
            this._unitCount[i].Encode(encoder);
        }

        encoder.WriteInt(this._spellsCount.Count);
        for (int i = 0; i < this._spellsCount.Count; i++)
        {
            this._spellsCount[i].Encode(encoder);
        }

        encoder.WriteInt(this._unitUpgrade.Count);
        for (int i = 0; i < this._unitUpgrade.Count; i++)
        {
            this._unitUpgrade[i].Encode(encoder);
        }

        encoder.WriteInt(this._spellUpgrade.Count);
        for (int i = 0; i < this._spellUpgrade.Count; i++)
        {
            this._spellUpgrade[i].Encode(encoder);
        }

        encoder.WriteInt(this._heroUpgrade.Count);
        for (int i = 0; i < this._heroUpgrade.Count; i++)
        {
            this._heroUpgrade[i].Encode(encoder);
        }

        encoder.WriteInt(this._heroHealth.Count);
        for (int i = 0; i < this._heroHealth.Count; i++)
        {
            this._heroHealth[i].Encode(encoder);
        }

        encoder.WriteInt(this._heroState.Count);
        for (int i = 0; i < this._heroState.Count; i++)
        {
            this._heroState[i].Encode(encoder);
        }

        encoder.WriteInt(this._allianceUnitCount.Count);
        for (int i = 0; i < this._allianceUnitCount.Count; i++)
        {
            this._allianceUnitCount[i].Encode(encoder);
        }

        encoder.WriteInt(this._missionCompleted.Count);
        for (int i = 0; i < this._missionCompleted.Count; i++)
        {
            ByteStreamHelper.WriteDataReference(encoder, this._missionCompleted[i]);
        }


        //encoder.WriteInt(TutorialSteps.Length);
        //foreach (int step in TutorialSteps)
        //{
        //    encoder.WriteInt(step);
        //}

        // TODO:
        encoder.WriteInt(0); // this._achievementRewardClaimed
        encoder.WriteInt(0); // this._achievementProgress
        encoder.WriteInt(0); // this._npcStars;
        encoder.WriteInt(0); // this._lootedNpcGold
        encoder.WriteInt(0); // this._lootedNpcElixir
    }

    public bool HasEnoughDiamonds(int count, bool callListener, LogicLevel level)
    {
        bool enough = this._diamonds >= count;

        if (!enough && callListener)
        {
            // TODO: level.GetGameListener().NotEnoughDiamonds();
        }

        return enough;
    }
}
