using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Net;
using System.Net.Sockets;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Server.Network.Connection;
using Wisedev.Magic.Server.Network.Extensions;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Network.TCP;

internal class TCPGateway : IServerGateway
{
    private Socket _socket;

    private Task _listenTask;
    private CancellationTokenSource _tokenSource;
    private ClientConnectionManager _manager;

    public TCPGateway()
    {
        this._socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this._tokenSource = new();

        var accountRepository = new MongoAccountRepository(new MongoClient("mongodb://localhost:27017"), "MagicDatabase");
        this._manager = new(accountRepository);
    }

    public void Start()
    {
        _socket.Bind(new IPEndPoint(IPAddress.Any, 9339));
        _socket.Listen(100);

        Debugger.Print($"Started tcp//0.0.0.0:9339");

        _listenTask = HandleAsync(_tokenSource.Token);
    }

    private async Task HandleAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Socket? client = await _socket.AcceptSocketAsync(token);
            if (client == null) break;

            _manager.OnConnect(client);
        }
    }

    public async Task Stop()
    {
        if (this._listenTask != null)
        {
            await this._tokenSource.CancelAsync();
            await this._listenTask!;
        }

        this._socket.Close();
    }
}
