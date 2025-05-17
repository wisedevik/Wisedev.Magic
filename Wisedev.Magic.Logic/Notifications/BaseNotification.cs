using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Notifications;
 
public class BaseNotification : LogicBase
{
    public virtual void Encode(ChecksumEncoder checksumEncoder)
    {
        base.Encode(checksumEncoder);
    }

    public virtual int GetNotificationType()
    {
        return -1;
    }
}
