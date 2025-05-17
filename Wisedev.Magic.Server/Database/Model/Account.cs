using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Titan.Logic;

namespace Wisedev.Magic.Server.Database.Model;

public class Account
{
    [BsonId]
    public ObjectId InternalId { get; set; }
    public LogicLong Id { get; set; }
    public string PassToken { get; set; }
    public int SessionCount { get; set; }
    public int PlayTimeSeconds { get; set; }
    public int DaysSinceStartedPlaying { get; set; }
    public int StartupCooldownSeconds { get; set; }
    public LogicClientHome Home { get; set; }
    public LogicClientAvatar ClientAvatar { get; set; }
    public int LastSeenNews { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime LastLoginAt { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime LastSaveTime { get; set; }

    public int ReportCount { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? BanEndTime { get; set; }
}
