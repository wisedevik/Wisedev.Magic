using Wisedev.Magic.Server.Network.Connection;
using Wisedev.Magic.Titan.Crypto;
using Wisedev.Magic.Titan.Message;
using Wisedev.Magic.Logic.Message;
using Wisedev.Magic.Titan.Debug;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Concurrent;
using Wisedev.Magic.Logic.Message.Home;

namespace Wisedev.Magic.Server.Protocol;

class Messaging : IConnectionListener
{
    public const int HEADER_SIZE = 7;
    
    private readonly ClientConnection _connection;
    private ConcurrentQueue<PiranhaMessage> _incomingQueue;
    private ConcurrentQueue<PiranhaMessage> _outgoingQueue;

    private StreamEncrypter _receiveEncrypter;
    private StreamEncrypter _sendEncrypter;

    private readonly LogicMessageFactory _factory;

    public Messaging(ClientConnection connection)
    {
        this._connection = connection;
        this._factory = LogicMagicMessageFactory.Instance;

        _incomingQueue = new ConcurrentQueue<PiranhaMessage>();
        _outgoingQueue = new ConcurrentQueue<PiranhaMessage>();

        this.InitializeEncryption();
    }

    public void InitializeEncryption()
    {
        if (this._receiveEncrypter != null)
            this._receiveEncrypter.Destruct();
        if (this._sendEncrypter != null)
            this._sendEncrypter.Destruct();

        this._receiveEncrypter = new RC4Encrypter(LogicMagicMessageFactory.RC4_KEY, "nonce");
        this._sendEncrypter = new RC4Encrypter(LogicMagicMessageFactory.RC4_KEY, "nonce");
    }

    public PiranhaMessage? NextMessage()
    {
        if (_incomingQueue.TryDequeue(out PiranhaMessage? message))
        {
            return message;
        }
        return null;
    }

    public int OnReceive(byte[] buffer, int length)
    {
        if (length >= Messaging.HEADER_SIZE)
        {
            Messaging.ReadHeader(buffer, out int messageType, out int messageLength, out int messageVersion);

            if (length - Messaging.HEADER_SIZE >= messageLength)
            {
                byte[] encryptedBytes = new byte[messageLength];
                byte[] encodingBytes = new byte[messageLength];

                Buffer.BlockCopy(buffer, Messaging.HEADER_SIZE, encryptedBytes, 0, messageLength);
                int encodingLength = messageLength;

                this._receiveEncrypter.Decrypt(encryptedBytes, encodingBytes, encodingLength);

                PiranhaMessage? message = this._factory.CreateMessageByType(messageType);
                if (message != null)
                {
                    message.GetByteStream().SetByteArray(encodingBytes, encodingLength);
                    message.SetMessageVersion(messageVersion);
                    try
                    {
                        message.Decode();

                        if (this._incomingQueue.Count >= 50)
                        {
                            Debugger.Warning($"Incoming message queue full. Message of type {messageType} discarded.");
                        }
                        else
                        {
                            if (!message.IsServerToClientMessage())
                            {
                                this._incomingQueue.Enqueue(message);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Debugger.Error(string.Format("Messaging.OnReceive: error while the decoding of message type {0}, trace: {1}", messageType, exception));
                    }
                }
                else
                {
                    Debugger.Warning($"Ignoring message of unknown type {messageType}");
                }

                return messageLength + Messaging.HEADER_SIZE;
            }
        }

        return 0;
    }

    public async Task Send(PiranhaMessage message)
    {
        int messageType = message.GetMessageType();

        if (!_connection.IsConnected())
        {
            Debugger.Warning($"Messaging.Send message type {messageType} when not connected");
        }

        if (message.IsServerToClientMessage())
        {
            Debugger.Warning($"Messaging.Send server to client message type {messageType} to {_connection.GetCurrentAccountId()}");
        }

        if (_outgoingQueue.Count >= 50)
        {
            Debugger.Warning($"Outgoing message queue full. Message of type {messageType} discarded.");
        }
        else
        {
            _outgoingQueue.Enqueue(message);
        }
    }

    public async Task OnWakeup()
    {
        while (this._outgoingQueue.TryDequeue(out PiranhaMessage message))
        {
            if (message.GetEncodingLength() == 0)
                message.Encode();

            int encodingLength = message.GetEncodingLength();
            int encryptedLength = encodingLength;

            byte[] encodingBytes = message.GetMessageBytes();

            byte[] encryptedBytes = new byte[encodingBytes.Length];
            _sendEncrypter.Encrypt(encodingBytes, encryptedBytes, encodingLength);

            byte[] stream = new byte[encryptedLength + Messaging.HEADER_SIZE];
            Messaging.WriteHeader(message, stream, encryptedLength);

            Buffer.BlockCopy(encryptedBytes, 0, stream, Messaging.HEADER_SIZE, encryptedLength);
            await this._connection.Send(stream);
        }
    }

    private static void WriteHeader(PiranhaMessage message, byte[] stream, int length)
    {
        int messageType = message.GetMessageType();
        int messageVersion = message.GetMessageVersion();

        stream[0] = (byte)(messageType >> 8);
        stream[1] = (byte)messageType;
        stream[2] = (byte)(length >> 16);
        stream[3] = (byte)(length >> 8);
        stream[4] = (byte)length;
        stream[5] = (byte)(messageVersion >> 8);
        stream[6] = (byte)messageVersion;
    }

    private static void ReadHeader(byte[] stream, out int messageType, out int length, out int messageVersion)
    {
        messageType = (stream[0] << 8) | stream[1];
        length = (stream[2] << 16) | (stream[3] << 8) | stream[4];
        messageVersion = (stream[5] << 8) | stream[6];
    }
}
