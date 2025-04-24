using System.Net.Sockets;
using System.Threading.Tasks;
using Wisedev.Magic.Logic.Mode;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Server.Database.Model;
using Wisedev.Magic.Server.Lgoic.Mode;
using Wisedev.Magic.Server.Protocol;
using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Message;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Network.Connection;

public class ClientConnection
{
    private Socket _socket;
    private Messaging _messaging;
    private MessageManager _messageManager;
    private ClientConnectionManager _manager;

    private byte[] _receiveBuffer;

    private LogicLong _currentAccountId;
    private GameMode _gameMode;

    private Account _accountDocument;

    public ClientConnection(Socket socket, ClientConnectionManager manager, IAccountRepository accountRepository, IAllianceRepository allianceRepository)
    {
        this._socket = socket;
        this._messaging = new Messaging(this);
        this._manager = manager;
        this._messageManager = new MessageManager(this, accountRepository, allianceRepository);
        this._receiveBuffer = GC.AllocateUninitializedArray<byte>(4096 * 2);
        this._gameMode = new GameMode(this);
    }

    public GameMode GetGameMode()
    {
        return this._gameMode;
    }

    public ClientConnectionManager GetClientConnectionManager()
    {
        return this._manager;
    }

    public void SetAccountDocument(Account acc)
    {
        this._accountDocument = acc;
    }

    public Account GetAccountDocument()
    {
        return this._accountDocument;
    }

    public void SetGameMode(GameMode mode)
    {
        this._gameMode = mode;
    }

    public bool IsConnected()
    {
        return _socket.Connected;
    }

    public void SetCurrentAccountId(LogicLong accountId)
    {
        _currentAccountId = accountId;
        _manager.AddConnection(this);
    }

    public LogicLong GetCurrentAccountId()
    {
        return _currentAccountId;
    }

    public async Task Receive()
    {
        int recvIdx = 0;
        Memory<byte> recvBufferMem = _receiveBuffer.AsMemory();

        while (true)
        {
            int r = await _socket.ReceiveAsync(recvBufferMem[recvIdx..], SocketFlags.None);
            if (r == 0)
                break;

            recvIdx += r;
            int consumedBytes = _messaging.OnReceive(_receiveBuffer, recvIdx);

            if (consumedBytes > 0)
            {
                Buffer.BlockCopy(_receiveBuffer, consumedBytes, _receiveBuffer, 0, recvIdx - consumedBytes);
                recvIdx -= consumedBytes;
            }
            else if (consumedBytes < 0)
            {
                break;
            }

            PiranhaMessage? message = this._messaging.NextMessage();
            if (message != null)
            {
                await this._messageManager.ReceiveMessage(message);
            }

            await this._messaging.OnWakeup();
        }
    }
    
    public async Task SendMessage(PiranhaMessage message)
    {
        await _messaging.Send(message);
    }

    public async Task Send(byte[] buffer)
    {
        await _socket.SendAsync(buffer, SocketFlags.None);
    }
}
