using MongoDB.Driver;
using Wisedev.Magic.Logic.Command.Home;
using Wisedev.Magic.Logic.Command.Listener;
using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Titam.Logic;

namespace Wisedev.Magic.Server.Database;

public class CommandVisitor : ICommandVisitor
{
    private readonly IAccountRepository _repository;
    private readonly LogicLong _accountId;

    public CommandVisitor(IAccountRepository repo, LogicLong accountId)
    {
        _repository = repo;
        _accountId = accountId;
    }

    public void Visit(LogicNewsSeenCommand command)
    {
        _repository.UpdateAccountAsync(
            _accountId,
            Builders<Account>.Update.Set(a => a.LastSeenNews, command.GetLastSeenNews())
        ).Wait();
    }
}
