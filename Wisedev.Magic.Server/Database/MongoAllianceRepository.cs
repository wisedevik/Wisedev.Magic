using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Titan.Logic;

public class MongoAllianceRepository : IAllianceRepository
{
    private readonly IMongoCollection<Alliance> _collection;

    public MongoAllianceRepository(IMongoClient mongoClient,
        string databaseName)
    {
        _collection = mongoClient.GetDatabase(databaseName).GetCollection<Alliance>("alliances");

        EnsureIndexes();
    }

    private void EnsureIndexes()
    {
        var indexKeys = Builders<Alliance>
            .IndexKeys
            .Ascending(a => a.AllianceId.High)
            .Ascending(a => a.AllianceId.Low);

        var indexOptions = new CreateIndexOptions { Background = false };
        _collection.Indexes.CreateOne(
            new CreateIndexModel<Alliance>(indexKeys, indexOptions),
            new CreateOneIndexOptions { MaxTime = TimeSpan.FromSeconds(30) });
    }

    public async Task<Alliance> CreateAsync(string name, string description)
    {
        var alliance = new Alliance
        {
            InternalId = ObjectId.GenerateNewId(),
            AllianceId = await GenerateNewLogicLongId(),
            Name = name,
            Description = description,
            Level = 1,
            Members = new List<LogicLong>()
        };

        await _collection.InsertOneAsync(alliance);
        return alliance;
    }

    public async Task<bool> ExistsAsync(long id)
    {
        return await _collection.Find(a => a.AllianceId == id).AnyAsync();
    }

    public async Task<Alliance?> GetByIdAsync(LogicLong allianceId)
    {
        return await _collection.Find(a => a.AllianceId == allianceId).FirstOrDefaultAsync();
    }

    public async Task UpdateAllianceAsync(LogicLong allianceId, UpdateDefinition<Alliance> update)
    {
        var filter = Builders<Alliance>.Filter.Eq(a => a.AllianceId, allianceId);
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task DeleteAllianceAsync(LogicLong allianceId)
    {
        var filter = Builders<Alliance>.Filter.Eq(a => a.AllianceId, allianceId);
        await _collection.DeleteOneAsync(filter);
    }

    public async Task<bool> AddMemberAsync(LogicLong allianceId, LogicLong memberId)
    {
        var filter = Builders<Alliance>.Filter.Eq(a => a.AllianceId, allianceId);
        var update = Builders<Alliance>.Update.AddToSet(a => a.Members, memberId);
        var result = await _collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    private async Task<LogicLong> GenerateNewLogicLongId()
    {
        var counters = _collection.Database.GetCollection<BsonDocument>("counters");

        var filter = Builders<BsonDocument>.Filter.Eq("_id", "allianceId");
        var update = Builders<BsonDocument>.Update.Inc("seq", 1);
        var options = new FindOneAndUpdateOptions<BsonDocument>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true
        };

        var result = await counters.FindOneAndUpdateAsync(filter, update, options);
        var sequence = result["seq"].AsInt32;

        return new LogicLong(0, sequence);
    }
}
