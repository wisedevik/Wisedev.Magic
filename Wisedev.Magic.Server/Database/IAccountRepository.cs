﻿using MongoDB.Driver;
using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Titan.Logic;

namespace Wisedev.Magic.Server.Database;

public interface IAccountRepository
{
    Task<Account> CreateAsync();
    Task<Account?> GetByIdAsync(LogicLong accountId);
    Task UpdateAccountAsync(LogicLong accountId, UpdateDefinition<Account> update);
    Task UpdateAccountAsync(Account account);

}
