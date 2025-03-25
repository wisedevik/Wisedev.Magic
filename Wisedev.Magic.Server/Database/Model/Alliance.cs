using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Wisedev.Magic.Titam.Logic;

namespace Wisedev.Magic.Server.Database.Model;

public class Alliance
{
    [BsonId]
    public ObjectId InternalId { get; set; }

    public LogicLong AllianceId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Level { get; set; } = 1;

    public List<LogicLong> Members { get; set; } = new();
}
