using System.Net.Sockets;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Network.Connection;

internal class ClientConnectionManager
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAllianceRepository _allianceRepository;

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

    private async Task RunSessionAsync(Socket client)
    {
        ClientConnection session = new ClientConnection(client, this._accountRepository, this._allianceRepository);
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
