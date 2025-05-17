using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Logic.Notifications;

public class BattleNotification : BaseNotification
{
    public string AttackingAvatarName { get; set; }

    public override void Encode(ChecksumEncoder checksumEncoder)
    {
        base.Encode(checksumEncoder);
        checksumEncoder.WriteString(AttackingAvatarName);
    }

    public override int GetNotificationType()
    {
        return 1;
    }
}
