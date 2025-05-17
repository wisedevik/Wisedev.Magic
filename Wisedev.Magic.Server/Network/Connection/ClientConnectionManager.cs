using System.Collections.Concurrent;
using System.Net.Sockets;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Server.Logic.Chat;
using Wisedev.Magic.Titan.Debug;
using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Message;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Wisedev.Magic.Server.Network.Connection;

public class ClientConnectionManager
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAllianceRepository _allianceRepository;
    private readonly ConcurrentDictionary<LogicLong, ClientConnection> _activeConnections = new();
    private readonly GlobalChat _chatInstance;

    public ClientConnectionManager(IAccountRepository accountRepository, IAllianceRepository allianceRepository)
    {
        this._accountRepository = accountRepository;
        this._allianceRepository = allianceRepository;
        this._chatInstance = new GlobalChat();
    }

    public int GetOnlineCount() => _activeConnections.Count;

    public ConcurrentDictionary<LogicLong, ClientConnection> GetActiveConnections() => _activeConnections;

    public void OnConnect(Socket client)
    {
        Debugger.Print($"New connection from {client.RemoteEndPoint}");
        _ = RunSessionAsync(client);
    }

    public void AddConnection(ClientConnection connection)
    {
        if (connection.GetCurrentAccountId() != null)
        {
            _activeConnections.TryAdd(connection.GetCurrentAccountId(), connection);
        }
    }

    private async Task RunSessionAsync(Socket client)
    {
        ClientConnection session = new ClientConnection(client, this, this._accountRepository, this._allianceRepository, this._chatInstance);
        try
        {
            await session.Receive();
        }
        catch (OperationCanceledException ex) { }
        catch (Exception ex)
        {
            Debugger.Error($"Unhandled exception occurred while processing session, trace:\n{ex}");
        }
        finally
        {
            Debugger.Warning("User has disconnected");
            _chatInstance.Remove(session);
            client.Dispose();
        }
    }
}
