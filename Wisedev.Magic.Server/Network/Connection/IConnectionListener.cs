namespace Wisedev.Magic.Server.Network.Connection;

interface IConnectionListener
{
    public int OnReceive(byte[] buffer, int length);
    public Task OnWakeup();
}
