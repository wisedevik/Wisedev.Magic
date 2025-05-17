using System.Threading.Tasks;
using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Message.Home;
using Wisedev.Magic.Server.Network.Connection;
using Wisedev.Magic.Titan.DataStream;

namespace Wisedev.Magic.Server.Logic.Chat;

public class GlobalChat
{
    private const int MAX_CHAT_LINES = 100;
    private readonly Dictionary<long, ClientConnection> _sessions;

    public GlobalChat()
    {
        _sessions = new Dictionary<long, ClientConnection>(MAX_CHAT_LINES);
    }

    public void Add(ClientConnection session)
    {
        if (this.IsFull())
            throw new Exception("GlobalChat.Add: instance is full");
        _sessions.Add(session.GetCurrentAccountId(), session);
    }

    public void Remove(ClientConnection session)
    {
        if (!_sessions.Remove(session.GetCurrentAccountId()))
            throw new Exception("GlobalChat.Remove: session is not in instance");
    }


    public bool IsFull()
    {
        return _sessions.Count >= MAX_CHAT_LINES;
    }

    public async Task PublishMessage(LogicClientAvatar logicClientAvatar, string message)
    {
        GlobalChatLineMessage globalChatLineMessage = new GlobalChatLineMessage();

        globalChatLineMessage.SetMessage(CensorUtil.FilterMessage(message));
        globalChatLineMessage.SetAvatarName(logicClientAvatar.GetName());
        globalChatLineMessage.SetExpLvl(logicClientAvatar.GetExpLevel());
        globalChatLineMessage.SetLeagueType(logicClientAvatar.LeagueType);
        globalChatLineMessage.SetAvatarId(logicClientAvatar.Id);
        globalChatLineMessage.SetHomeId(logicClientAvatar.CurrentHomeId);

        if (logicClientAvatar.IsInAlliance())
        {
            globalChatLineMessage.SetAllianceId(logicClientAvatar.AllianceId);
        }

        

        foreach (ClientConnection session in _sessions.Values)
        {
            Console.WriteLine($"{globalChatLineMessage.ToString()}");
            await session.SendMessage(globalChatLineMessage);
        }
    }
}
