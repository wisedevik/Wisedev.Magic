using System.Collections.Concurrent;
using System.Net.Sockets;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Titan.Debug;
using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Server.Network.Connection;

public class ClientConnectionManager
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAllianceRepository _allianceRepository;
    private readonly ConcurrentDictionary<LogicLong, ClientConnection> _activeConnections = new();

    public ClientConnectionManager(IAccountRepository accountRepository, IAllianceRepository allianceRepository)
    {
        this._accountRepository = accountRepository;
        this._allianceRepository = allianceRepository;
    }

    public void OnConnect(Socket client)
    {
        Debugger.Print($"New connection from {client.RemoteEndPoint}");
        _ = RunSessionAsync(client);
    }

    public async Task BroadcastMessage(PiranhaMessage message)
    {
        foreach (var connection in _activeConnections.Values.ToArray())
        {
            if (connection.IsConnected())
            {
                try
                {
                    await connection.SendMessage(message);
                }
                catch (Exception ex)
                {
                    Debugger.Error($"Error broadcasting message: {ex.Message}");
                }
            }
        }
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
        ClientConnection session = new ClientConnection(client, this, this._accountRepository, this._allianceRepository);
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
            client.Dispose();
        }
    }
}
