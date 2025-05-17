using Wisedev.Magic.Logic.Notifications;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message.Home;

public class NotificationMessage : PiranhaMessage
{
    public const int MESSAGE_TYPE = 20801;

    public NotificationMessage() : base(0)
    {
    }

    public BaseNotification Notification { get; set; }

    public override void Encode()
    {
        base.Encode();
        _stream.WriteInt(Notification.GetNotificationType());
        Notification.Encode(_stream);
    }

    public override short GetMessageType()
    {
        return MESSAGE_TYPE;
    }
}
