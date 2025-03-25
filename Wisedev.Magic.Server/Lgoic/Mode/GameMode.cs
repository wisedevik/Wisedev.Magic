using System.Threading.Tasks;
using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Message.Home;
using Wisedev.Magic.Logic.Mode;
using Wisedev.Magic.Server.Network.Connection;
using Wisedev.Magic.Titam.Utils;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Lgoic.Mode;

public class GameMode
{
    private readonly LogicGameMode _logicGameMode;

    public GameMode()
    {
        _logicGameMode = new LogicGameMode();
    }

    public static async ValueTask<GameMode> LoadHomeState(ClientConnection session, LogicClientHome logicHome, LogicAvatar logicAvatar)
    {
        LogicClientHome home = logicHome;
        LogicClientAvatar homeOwnerAvatar = (LogicClientAvatar)logicAvatar;

        OwnHomeDataMessage ohd = new OwnHomeDataMessage();

        ohd.SetLogicClientHome(home);
        ohd.SetLogicClientAvatar(homeOwnerAvatar);
        ohd.SetSecondsSinceLastSave(0);

        try
        {
            GameMode gameMode = new GameMode();

            //TODO: set listeners

            gameMode._logicGameMode.LoadHomeState(home, homeOwnerAvatar, 0);

            await session.SendMessage(ohd);

            return gameMode;
        }
        catch (Exception e)
        {
            Debugger.Error(e.Message);
        }

        return null;
    }

    public void OnClientTurnReceived(int subTick, int checksum, List<LogicCommand> commands)
    {
        if (this._logicGameMode.GetState() == 4 || this._logicGameMode.GetState() == 5)
            return;

        if (commands != null)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                this._logicGameMode.GetCommandManager().AddCommand(commands[i]);
            }
        }

        int prevSubTick = this._logicGameMode.GetLevel().GetLogicTime().GetTick();

        for (int i = 0, cnt = subTick - prevSubTick; i < cnt; i++)
        {
            this._logicGameMode.UpdateOneSubTick();
        }
    }
}
