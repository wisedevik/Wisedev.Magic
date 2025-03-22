using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Titam.Logic;

namespace Wisedev.Magic.Server.Database.Model;

class Account
{
    [BsonId]
    public ObjectId InternalId { get; set; }
    public LogicLong Id { get; set; }
    public string PassToken { get; set; }
    public LogicClientHome Home { get; set; }
    public LogicClientAvatar ClientAvatar { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
