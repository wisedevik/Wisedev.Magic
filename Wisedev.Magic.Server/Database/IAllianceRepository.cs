
using MongoDB.Driver;
using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Titam.Logic;

namespace Wisedev.Magic.Server.Database;

public interface IAllianceRepository
{
    Task<Alliance> CreateAsync(string name, string description);
    Task<Alliance?> GetByIdAsync(LogicLong allianceId);
    Task UpdateAllianceAsync(LogicLong allianceId, UpdateDefinition<Alliance> update);
    Task DeleteAllianceAsync(LogicLong allianceId);
    Task<bool> AddMemberAsync(LogicLong allianceId, LogicLong memberId);
}
