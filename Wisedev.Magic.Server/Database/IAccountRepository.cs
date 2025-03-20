using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Titam.Logic;

namespace Wisedev.Magic.Server.Database;

interface IAccountRepository
{
    Task<Account> CreateAsync();
    Task<Account?> GetByIdAsync(LogicLong accountId);
}
