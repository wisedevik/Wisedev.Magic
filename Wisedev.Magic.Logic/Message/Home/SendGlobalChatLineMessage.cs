using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

[PiranhaMessage(SendGlobalChatLineMessage.MESSAGE_TYPE)]
public class SendGlobalChatLineMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 14715;

    private string? _message;

    public SendGlobalChatLineMessage() : base(0)
    {
        this._message = null;
    }

    public override void Decode()
    {
        base.Decode();
        this._message = this._stream.ReadString();
    }

    public string? GetMessage()
    {
        return this._message;
    }

    public override short GetMessageType()
    {
        return SendGlobalChatLineMessage.MESSAGE_TYPE;
    }
}
