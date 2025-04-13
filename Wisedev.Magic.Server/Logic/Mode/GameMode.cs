using MongoDB.Driver.Core.Bindings;
using System.Threading.Tasks;
using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Command;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Message.Home;
using Wisedev.Magic.Logic.Mode;
using Wisedev.Magic.Server.Lgoic.Mode.Listener;
using Wisedev.Magic.Server.Network.Connection;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Utils;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Lgoic.Mode;

public class GameMode
{
    private readonly ClientConnection _connection;
    private readonly LogicGameMode _logicGameMode;
    private AvatarChangeListener _avatarChangeListener;

    public GameMode(ClientConnection connection)
    {
        _logicGameMode = new LogicGameMode();
        _connection = connection;
    }

    public LogicGameMode GetLogicGameMode()
    {
        return this._logicGameMode;
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
            GameMode gameMode = new GameMode(session);

            gameMode._avatarChangeListener = new AvatarChangeListener();

            homeOwnerAvatar.SetChangeListener(gameMode._avatarChangeListener);

            gameMode._logicGameMode.LoadHomeState(home, homeOwnerAvatar, 0);

            await session.SendMessage(ohd);

            return gameMode;
        }
        catch (Exception e)
        {
            Debugger.Error($"{e.Message}\n{e.StackTrace}");
        }
        return null;
    }

    public static async ValueTask<GameMode?> LoadNpcAttackState(ClientConnection session, 
        LogicClientHome home, 
        LogicClientAvatar visitorAvatar, 
        LogicNpcAvatar npcAvatar)
    {
        NpcDataMessage npcDataMessage = new NpcDataMessage();

        npcDataMessage.SetSecondsSinceLastSave(0);
        npcDataMessage.SetLevelJSON("""
            
            
{         
  "buildings": [
    {
      "data": 1000001,
      "lvl": 0,
      "x": 21,
      "y": 20
    },
    {
      "data": 1000004,
      "lvl": 0,
      "x": 20,
      "y": 16,
      "res_time": 8992
    },
    {
      "data": 1000000,
      "lvl": 0,
      "x": 26,
      "y": 19,
      "units": [],
      "storage_type": 0
    },
    {
      "data": 1000015,
      "lvl": 0,
      "x": 18,
      "y": 20
    },
    {
      "data": 1000014,
      "lvl": 0,
      "locked": true,
      "x": 25,
      "y": 32
    }
  ],
  "obstacles": [
    {
      "data": 8000007,
      "x": 5,
      "y": 13
    },
    {
      "data": 8000007,
      "x": 15,
      "y": 29
    },
    {
      "data": 8000008,
      "x": 7,
      "y": 7
    },
    {
      "data": 8000005,
      "x": 29,
      "y": 4
    },
    {
      "data": 8000006,
      "x": 15,
      "y": 37
    },
    {
      "data": 8000000,
      "x": 20,
      "y": 4
    },
    {
      "data": 8000008,
      "x": 15,
      "y": 22
    },
    {
      "data": 8000005,
      "x": 37,
      "y": 18
    },
    {
      "data": 8000007,
      "x": 6,
      "y": 4
    },
    {
      "data": 8000003,
      "x": 26,
      "y": 10
    },
    {
      "data": 8000004,
      "x": 21,
      "y": 9
    },
    {
      "data": 8000008,
      "x": 32,
      "y": 21
    },
    {
      "data": 8000005,
      "x": 20,
      "y": 36
    },
    {
      "data": 8000003,
      "x": 29,
      "y": 34
    },
    {
      "data": 8000005,
      "x": 5,
      "y": 29
    },
    {
      "data": 8000005,
      "x": 8,
      "y": 10
    },
    {
      "data": 8000005,
      "x": 5,
      "y": 17
    },
    {
      "data": 8000002,
      "x": 4,
      "y": 33
    },
    {
      "data": 8000002,
      "x": 5,
      "y": 21
    },
    {
      "data": 8000002,
      "x": 10,
      "y": 32
    },
    {
      "data": 8000008,
      "x": 5,
      "y": 37
    },
    {
      "data": 8000001,
      "x": 9,
      "y": 4
    },
    {
      "data": 8000001,
      "x": 13,
      "y": 31
    },
    {
      "data": 8000001,
      "x": 7,
      "y": 35
    },
    {
      "data": 8000007,
      "x": 4,
      "y": 9
    },
    {
      "data": 8000004,
      "x": 9,
      "y": 23
    },
    {
      "data": 8000004,
      "x": 6,
      "y": 26
    },
    {
      "data": 8000003,
      "x": 35,
      "y": 21
    },
    {
      "data": 8000005,
      "x": 32,
      "y": 28
    },
    {
      "data": 8000005,
      "x": 34,
      "y": 13
    },
    {
      "data": 8000001,
      "x": 14,
      "y": 18
    },
    {
      "data": 8000001,
      "x": 35,
      "y": 5
    },
    {
      "data": 8000012,
      "x": 24,
      "y": 30
    },
    {
      "data": 8000012,
      "x": 31,
      "y": 10
    },
    {
      "data": 8000010,
      "x": 26,
      "y": 38
    },
    {
      "data": 8000010,
      "x": 14,
      "y": 5
    },
    {
      "data": 8000013,
      "x": 34,
      "y": 33
    },
    {
      "data": 8000013,
      "x": 13,
      "y": 9
    },
    {
      "data": 8000014,
      "x": 10,
      "y": 17
    },
    {
      "data": 8000014,
      "x": 24,
      "y": 7
    },
    {
      "data": 8000006,
      "x": 36,
      "y": 26
    },
    {
      "data": 8000011,
      "x": 23,
      "y": 34
    },
    {
      "data": 8000011,
      "x": 24,
      "y": 37
    },
    {
      "data": 8000000,
      "x": 27,
      "y": 35
    },
    {
      "data": 8000000,
      "x": 25,
      "y": 35
    },
    {
      "data": 8000000,
      "x": 26,
      "y": 30
    },
    {
      "data": 8000007,
      "x": 23,
      "y": 32
    },
    {
      "data": 8000001,
      "x": 28,
      "y": 31
    },
    {
      "data": 8000014,
      "x": 28,
      "y": 29
    }
  ],
  "traps": [],
  "decos": [],
  "respawnVars": {
    "secondsFromLastRespawn": 0,
    "respawnSeed": 1529463799,
    "obstacleClearCounter": 0
  },
  "cooldowns": [],
}                  
""");
        npcDataMessage.SetLogicClientAvatar(visitorAvatar);
        npcDataMessage.SetLogicNpcAvatar(npcAvatar);

        try
        {
            GameMode gameMode = new GameMode(session);

            gameMode._logicGameMode.LoadNpcAttackState(home, npcAvatar, visitorAvatar, 0);

            await session.SendMessage(npcDataMessage);

            return gameMode;
        }
        catch (Exception e)
        {
            Debugger.Error("GameMode.LoadNpcAttackState: exception while the loading of attack state: " + e);
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

        this.SaveState();
    }

    private void SaveState()
    {
        if (this._logicGameMode.GetState() == 1)
        {
            LogicJSONObject jsonObject = new LogicJSONObject();
            this._logicGameMode.SaveToJSON(jsonObject);

            this._connection.GetAccountDocument().ClientAvatar = this._logicGameMode.GetLevel().GetPlayerAvatar();
            this._connection.GetAccountDocument().Home = this._logicGameMode.GetLevel().GetHome();
            this._connection.GetAccountDocument().Home.SetHomeJSON(LogicJSONParser.CreateJSONString(jsonObject));
        }
    }
}
