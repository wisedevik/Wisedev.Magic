﻿using MongoDB.Driver.Core.Bindings;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        logicAvatar.SetUnitCount(LogicDataTables.GetCharacterByName("Barbarian", null), 1488);

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

            DateTime lastSaveTime = session.GetAccountDocument().LastSaveTime;
            TimeSpan elapsed = DateTime.UtcNow - lastSaveTime;
            int secondsPassed = (int)elapsed.TotalSeconds;

            gameMode.GetLogicGameMode().GetLevel().GetGameObjectManager().AdjustConstructionTimers(secondsPassed);

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
{"buildings":[{"data":1000001,"lvl":0,"x":17,"y":20},{"data":1000008,"lvl":0,"x":22,"y":16},{"data":1000008,"lvl":0,"x":22,"y":25},{"data":1000010,"lvl":0,"x":24,"y":28},{"data":1000010,"lvl":0,"x":23,"y":28},{"data":1000010,"lvl":0,"x":25,"y":27},{"data":1000010,"lvl":0,"x":25,"y":26},{"data":1000010,"lvl":0,"x":25,"y":25},{"data":1000010,"lvl":0,"x":22,"y":28},{"data":1000010,"lvl":0,"x":21,"y":28},{"data":1000010,"lvl":0,"x":17,"y":28},{"data":1000010,"lvl":0,"x":19,"y":28},{"data":1000010,"lvl":0,"x":20,"y":28},{"data":1000010,"lvl":0,"x":16,"y":28},{"data":1000010,"lvl":0,"x":25,"y":18},{"data":1000010,"lvl":0,"x":25,"y":17},{"data":1000010,"lvl":0,"x":25,"y":16},{"data":1000010,"lvl":0,"x":25,"y":15},{"data":1000010,"lvl":0,"x":25,"y":24},{"data":1000010,"lvl":0,"x":25,"y":19},{"data":1000010,"lvl":0,"x":24,"y":15},{"data":1000010,"lvl":0,"x":23,"y":15},{"data":1000010,"lvl":0,"x":21,"y":15},{"data":1000010,"lvl":0,"x":22,"y":15},{"data":1000010,"lvl":0,"x":20,"y":15},{"data":1000010,"lvl":0,"x":19,"y":15},{"data":1000010,"lvl":0,"x":18,"y":15},{"data":1000010,"lvl":0,"x":17,"y":15},{"data":1000010,"lvl":0,"x":16,"y":15},{"data":1000010,"lvl":0,"x":15,"y":15},{"data":1000010,"lvl":0,"x":19,"y":29},{"data":1000010,"lvl":0,"x":15,"y":28},{"data":1000010,"lvl":0,"x":15,"y":27},{"data":1000010,"lvl":0,"x":14,"y":27},{"data":1000010,"lvl":0,"x":14,"y":26},{"data":1000010,"lvl":0,"x":14,"y":25},{"data":1000010,"lvl":0,"x":14,"y":24},{"data":1000010,"lvl":0,"x":14,"y":23},{"data":1000010,"lvl":0,"x":14,"y":22},{"data":1000010,"lvl":0,"x":14,"y":21},{"data":1000010,"lvl":0,"x":14,"y":20},{"data":1000010,"lvl":0,"x":14,"y":19},{"data":1000010,"lvl":0,"x":14,"y":18},{"data":1000010,"lvl":0,"x":14,"y":16},{"data":1000010,"lvl":0,"x":15,"y":16},{"data":1000010,"lvl":0,"x":17,"y":29},{"data":1000010,"lvl":0,"x":14,"y":17},{"data":1000010,"lvl":1,"x":30,"y":28},{"data":1000010,"lvl":1,"x":33,"y":28},{"data":1000010,"lvl":0,"x":26,"y":19},{"data":1000010,"lvl":1,"x":33,"y":25},{"data":1000010,"lvl":1,"x":33,"y":27},{"data":1000010,"lvl":1,"x":31,"y":28},{"data":1000010,"lvl":1,"x":32,"y":28},{"data":1000010,"lvl":1,"x":29,"y":24},{"data":1000010,"lvl":1,"x":29,"y":26},{"data":1000010,"lvl":1,"x":30,"y":24},{"data":1000010,"lvl":1,"x":31,"y":24},{"data":1000010,"lvl":1,"x":33,"y":24},{"data":1000010,"lvl":1,"x":32,"y":24},{"data":1000010,"lvl":1,"x":29,"y":25},{"data":1000010,"lvl":1,"x":29,"y":28},{"data":1000010,"lvl":1,"x":29,"y":27},{"data":1000010,"lvl":0,"x":26,"y":24},{"data":1000010,"lvl":0,"x":21,"y":23},{"data":1000010,"lvl":0,"x":21,"y":20},{"data":1000010,"lvl":0,"x":16,"y":23},{"data":1000010,"lvl":0,"x":16,"y":20},{"data":1000010,"lvl":0,"x":25,"y":28},{"data":1000018,"lvl":0,"x":15,"y":21,"units":[],"storage_type":0}],"obstacles":[{"data":8000000,"x":4,"y":24,"loot_multiply_ver":1},{"data":8000003,"x":6,"y":23,"loot_multiply_ver":1},{"data":8000002,"x":8,"y":34,"loot_multiply_ver":1},{"data":8000003,"x":31,"y":14,"loot_multiply_ver":1},{"data":8000012,"x":14,"y":30,"loot_multiply_ver":1},{"data":8000012,"x":19,"y":10,"loot_multiply_ver":1},{"data":8000010,"x":9,"y":9,"loot_multiply_ver":1},{"data":8000014,"x":12,"y":15,"loot_multiply_ver":1},{"data":8000006,"x":4,"y":16,"loot_multiply_ver":1},{"data":8000011,"x":21,"y":32,"loot_multiply_ver":1},{"data":8000000,"x":6,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":22,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":26,"loot_multiply_ver":1},{"data":8000014,"x":24,"y":32,"loot_multiply_ver":1},{"data":8000000,"x":34,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":8,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":36,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":30,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":36,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":26,"loot_multiply_ver":1},{"data":8000000,"x":12,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":28,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":26,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":34,"loot_multiply_ver":1},{"data":8000000,"x":16,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":14,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":18,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":22,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":30,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":28,"loot_multiply_ver":1},{"data":8000000,"x":24,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":32,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":10,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":28,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":32,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":30,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":18,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":34,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":36,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":32,"loot_multiply_ver":1},{"data":8000000,"x":20,"y":38,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":20,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":14,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":12,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":10,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":8,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":6,"loot_multiply_ver":1},{"data":8000000,"x":4,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":6,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":8,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":10,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":12,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":14,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":16,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":18,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":20,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":22,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":24,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":26,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":28,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":30,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":32,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":34,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":36,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":4,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":6,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":8,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":10,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":12,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":14,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":16,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":18,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":20,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":22,"loot_multiply_ver":1},{"data":8000000,"x":38,"y":24,"loot_multiply_ver":1},{"data":8000019,"x":23,"y":10,"loot_multiply_ver":1},{"data":8000019,"x":29,"y":32,"loot_multiply_ver":1},{"data":8000019,"x":32,"y":10,"loot_multiply_ver":1},{"data":8000009,"x":30,"y":27,"loot_multiply_ver":1},{"data":8000009,"x":31,"y":27,"loot_multiply_ver":1},{"data":8000009,"x":32,"y":27,"loot_multiply_ver":1},{"data":8000009,"x":32,"y":25,"loot_multiply_ver":1},{"data":8000009,"x":31,"y":25,"loot_multiply_ver":1},{"data":8000009,"x":30,"y":25,"loot_multiply_ver":1},{"data":8000009,"x":30,"y":26,"loot_multiply_ver":1},{"data":8000009,"x":32,"y":26,"loot_multiply_ver":1},{"data":8000023,"x":31,"y":26,"loot_multiply_ver":1}],"traps":[{"data":12000000,"lvl":0,"x":33,"y":26}],"decos":[{"data":18000001,"x":17,"y":17},{"data":18000001,"x":17,"y":25}],"respawnVars":{"secondsFromLastRespawn":0,"respawnSeed":162879964,"obstacleClearCounter":0,"time_to_gembox_drop":360000,"time_in_gembox_period":244800},"cooldowns":[],"newShopBuildings":[1,0,1,1,1,1,1,0,2,0,0,0,0,0,1,5,0,0,0,0,0,0,0,0,0,0,0,0],"newShopTraps":[0,0,0,0,0,0,0,0],"newShopDecos":[1,4,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],"last_league_rank":0,"last_league_shuffle":0,"last_news_seen":-1,"edit_mode_shown":false}                  
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
