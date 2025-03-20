using System.Net.Sockets;
using System.Threading.Tasks;
using Wisedev.Magic.Server.Database;
using Wisedev.Magic.Server.Protocol;
using Wisedev.Magic.Titam.Message;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Network.Connection;

class ClientConnection
{
    private Socket _socket;
    private Messaging _messaging;
    private MessageManager _messageManager;

    private byte[] _receiveBuffer;

    public ClientConnection(Socket socket, IAccountRepository accountRepository)
    {
        this._socket = socket;
        this._messaging = new Messaging(this);
        this._messageManager = new MessageManager(this, accountRepository);
        this._receiveBuffer = GC.AllocateUninitializedArray<byte>(4096);
    }

    public bool IsConnected()
    {
        return _socket.Connected;
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
