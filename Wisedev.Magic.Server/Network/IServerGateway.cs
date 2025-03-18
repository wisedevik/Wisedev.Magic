namespace Wisedev.Magic.Server.Network;

internal interface IServerGateway
{
    void Start();
    Task Stop();
}
