using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Server.Resources;
using Wisedev.Magic.Titam.Logic;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Avatar;

namespace Wisedev.Magic.Server.Database;

class MongoAccountRepository : IAccountRepository
{
    private readonly IMongoCollection<Account> _collection;

    public MongoAccountRepository(
        IMongoClient mongoClient,
        string databaseName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        _collection = database.GetCollection<Account>("accounts");

        EnsureIndexes();
    }

    private void EnsureIndexes()
    {
        var indexKeys = Builders<Account>
            .IndexKeys
            .Ascending(a => a.Id.High)
            .Ascending(a => a.Id.Low);

        var indexOptions = new CreateIndexOptions { Background = false };
        _collection.Indexes.CreateOne(
            new CreateIndexModel<Account>(indexKeys, indexOptions),
            new CreateOneIndexOptions { MaxTime = TimeSpan.FromSeconds(30) });
    }

    public async Task<Account> CreateAsync()
    {
        var account = new Account
        {
            InternalId = ObjectId.GenerateNewId(),
            Id = await GenerateNewLogicLongId(),
            PassToken = GenerateSecureToken(32),
            Home = new LogicClientHome(),
            ClientAvatar = LogicClientAvatar.GetDefaultAvatar(),
            LastLoginAt = DateTime.UtcNow,
            SessionCount = 1,
        };

        account.Home.SetId(account.Id);
        account.Home.SetHomeJSON(ResourceManager.STARTING_HOME_JSON!);
        account.ClientAvatar.SetId(account.Id);
        account.ClientAvatar.SetCurrentHomeId(account.Id);
        account.ClientAvatar.SetAllianceId(0);
        account.ClientAvatar.SetLastLeagueInstanceId(0);
        account.ClientAvatar.SetLeagueInstanceId(0);
        account.ClientAvatar.SetName("wisedev <3");
        account.ClientAvatar.SetExpLevel(1);

        await _collection.InsertOneAsync(account);
        return account;
    }

    public async Task<Account?> GetByIdAsync(LogicLong accountId)
    {
        return await _collection
            .Find(a => a.Id.High == accountId.High && a.Id.Low == accountId.Low)
            .FirstOrDefaultAsync();
    }

    private async Task<LogicLong> GenerateNewLogicLongId()
    {
        var counters = _collection.Database.GetCollection<BsonDocument>("counters");

        var filter = Builders<BsonDocument>.Filter.Eq("_id", "accountId");
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

    private static string GenerateSecureToken(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        var tokenData = new byte[length / 2];
        rng.GetBytes(tokenData);
        return BitConverter.ToString(tokenData).Replace("-", "").ToLower();
    }

    public async Task UpdateAccountAsync(LogicLong accountId, UpdateDefinition<Account> update)
    {
        var filter = Builders<Account>.Filter.Eq(a => a.Id, accountId);
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateAccountAsync(Account account)
    {
        var filter = Builders<Account>.Filter.Eq(a => a.Id, account.Id);
        await _collection.ReplaceOneAsync(filter, account, new ReplaceOptions { IsUpsert = false });
    }
}
