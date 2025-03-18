using System.Net.Sockets;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Network.Connection;

internal class ClientConnectionManager
{
    public ClientConnectionManager()
    {
        ;
    }

    public void OnConnect(Socket client)
    {
        Debugger.Print($"New connection from {client.RemoteEndPoint}");
        _ = RunSessionAsync(client);
    }

    private async Task RunSessionAsync(Socket client)
    {
        ClientConnection session = new ClientConnection(client);
        try
        {
            await session.Receive();
        }
        catch (OperationCanceledException ex) { }
        catch (Exception ex)
        {
            //Debugger.Error($"Unhandled exception occurred while processing session, trace:\n{ex}");
        }
        finally
        {
            Debugger.Warning("User has disconnected");
            client.Dispose();
        }
    }
}
