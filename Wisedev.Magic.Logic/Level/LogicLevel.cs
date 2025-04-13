using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Avatar.Listener;
using Wisedev.Magic.Logic.Battle;
using Wisedev.Magic.Logic.Coldown;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.GameObject;
using Wisedev.Magic.Logic.GameObject.Component;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Mode;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Logic.Worker;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Utils;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Level;

public class LogicLevel
{
    private LogicGameListener _gameListener;

    private bool _battleStarted;

    private LogicGameMode _gameMode;
    private LogicTime _logicTime;

    private int _lastSeenNews;

    private LogicAvatar _visitorAvatar;
    private LogicAvatar _homeOwnerAvatar;

    private LogicClientHome _home;
    private LogicBattleLog _battleLog;

    private LogicArrayList<int> _newShopBuildings;
    private LogicArrayList<int> _newShopTraps;
    private LogicArrayList<int> _newShopDecos;

    private LogicTileMap _tileMap;
    private LogicGameObjectManager _gameObjectManager;
    private LogicCooldownManager _cooldownManager;
    private LogicWorkerManager _workerManager;
    private LogicRect _playArea;
    private int _matchType;
    private LogicLong _revengeId;
    private bool _npcVillage;
    private int _waveNumber;
    private bool _androidClient;
    private int _lastLeagueRank;
    private int _lastLeagueShuffle;
    private bool _editModeShown;
    private string _troopReqMsg;


    public LogicLevel(LogicGameMode gameMode)
    {
        this._gameMode = gameMode;
        this._logicTime = new LogicTime();
        this._tileMap = new LogicTileMap(44, 44);
        this._playArea = new LogicRect(2, 2, 42, 42);
        this._gameObjectManager = new LogicGameObjectManager(this._tileMap, this);
        this._battleLog = new LogicBattleLog(this);
        this._workerManager = new LogicWorkerManager(this);
        this._cooldownManager = new LogicCooldownManager();

        this._newShopBuildings = new LogicArrayList<int>();
        this._newShopTraps = new LogicArrayList<int>();
        this._newShopDecos = new LogicArrayList<int>();

        this._troopReqMsg = string.Empty;
        this._lastSeenNews = -1;

        LogicDataTable buildingData = LogicDataTables.GetTable(LogicDataType.BUILDING);
        LogicDataTable trapData = LogicDataTables.GetTable(LogicDataType.TRAP);
        LogicDataTable decoData = LogicDataTables.GetTable(LogicDataType.DECO);

        this._newShopBuildings.EnsureCapacity(buildingData.GetItemCount());
        this._newShopTraps.EnsureCapacity(trapData.GetItemCount());
        this._newShopDecos.EnsureCapacity(decoData.GetItemCount());
    }

    public int GetState()
    {
        return _gameMode.GetState();
    }

    public LogicTime GetLogicTime()
    {
        return this._logicTime;
    }

    public void SetHome(LogicClientHome home)
    {
        this._home = home;

        LogicJSONObject homeJSON = LogicJSONParser.ParseObject(home.GetHomeJSON());

        LogicJSONBoolean androidClient = homeJSON.GetJSONBoolean("android_client");
        if (androidClient != null)
            this._androidClient = androidClient.IsTrue();

        LogicJSONNumber waveNumObject = homeJSON.GetJSONNumber("wave_num");

        if (waveNumObject != null)
        {
            if (this._gameMode.GetState() != 1)
                this._waveNumber = waveNumObject.GetIntValue();
        }

        this._gameObjectManager.Load(homeJSON);
    }

    public LogicGameObjectManager GetGameObjectManager()
    {
        return this._gameObjectManager;
    }

    public LogicClientHome GetHome()
    {
        return this._home;
    }

    public LogicAvatar GetVisitorAvatar()
    {
        return this._visitorAvatar;
    }

    public LogicAvatar GetHomeOwnerAvatar()
    {
        return this._homeOwnerAvatar;
    }

    public LogicWorkerManager GetWorkerManager()
    {
        return this._workerManager;
    }

    public void SetVisitorAvatar(LogicAvatar avatar)
    {
        this._visitorAvatar = avatar;
    }

    public void SetHomeOwnerAvatar(LogicAvatar avatar)
    {
        this._homeOwnerAvatar = avatar;

        // TODO: more logic
    }

    public bool IsValidPlaceForBuilding(int x, int y, int width, int height, LogicGameObject gameObject)
    {
        if (this._playArea.IsInside(x, y) && this._playArea.IsInside(x + width, y + height))
        {
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    if (!this._tileMap.GetTile(x + i, y + k).IsBuildable(gameObject))
                        return false;
                }
            }

            return true;
        }

        return true;
    }

    public void Tick()
    {
        int state = this.GetState();
        if (state == 2 && !this._battleStarted && this._battleLog.GetBattleStarted())
        {
            this.BattleStarted();
        }

        this._gameObjectManager.Tick();
        this._cooldownManager.Tick();
        /*TODO:
        LogicMissionManager::tick
        LogicAchievementManager::tick
        LogicNpcAttack::tick if != null
         */
    }

    public void SubTick()
    {

    }

    public void BattleStarted()
    {
        Debugger.Print("LogicLevel - battleStarted");
        this._battleStarted = true;
        if (this._matchType == 4 && !LogicDataTables.GetGlobals().RemoveRevengeWhenBattleIsLoaded())
        {
            LogicAvatar avatar = null;
            if (this._gameMode.GetState() == 1 || this._gameMode.GetState() == 3)
                avatar = this._homeOwnerAvatar;
            else
                avatar = this._visitorAvatar;

            LogicAvatarChangeListener changeListener = avatar.GetChangeListener();
            changeListener.RevengeUsed(this._revengeId);
            
            if (this._revengeId != 0)
            {
                Debugger.Print($"Revenge used {this._revengeId.GetHigherInt()}, {this._revengeId.GetHigherInt()}");
            }
        }
    }

    public void SetMatchType(int t, LogicLong id)
    {
        this._matchType = t;
        this._revengeId = id;

        if (t == 2)
            this._npcVillage = true;
    }

    public void SetNpcVillage(bool npcVillage)
    {
        this._npcVillage= npcVillage;
    }

    public void SetLastSeenNews(int lastSeenNews)
    {
        this._lastSeenNews = lastSeenNews;
    }

    public void SetGameListener(LogicGameListener gameListener)
    {
        this._gameListener = gameListener;
    }

    public LogicGameListener GetGameListener()
    {
        return this._gameListener;
    }

    public LogicClientAvatar GetPlayerAvatar()
    {
        if (this._gameMode.GetState() == 1
            || this._gameMode.GetState() == 3)
                return (LogicClientAvatar)this._homeOwnerAvatar;

        return (LogicClientAvatar)this._visitorAvatar;
    }

    public bool SetUnlockedShopItemCount(int dataType, int idx, int count)
    {
        switch (dataType)
        {
            case 0:
                this._newShopBuildings[idx] = count;
                break;
            case 11:
                this._newShopTraps[idx] = count;
                break;
            case 17:
                this._newShopDecos[idx] = count;
                break;
            default: return false;
        }

        return true;
    }

    public LogicTileMap GetTileMap()
    {
        return this._tileMap;
    }

    public LogicComponentManager GetComponentManager()
    {
        return this._gameObjectManager.GetComponentManager();
    }

    public bool IsInCombatState()
    {
        int state = this.GetState();
        return state == 2 || state == 3 || state == 5;
    }

    public void SaveToJSON(LogicJSONObject logicJSONObject)
    {
        if (this._waveNumber >= 1)
            logicJSONObject.Put("wave_num", new LogicJSONNumber(this._waveNumber));

        if (this._androidClient)
            logicJSONObject.Put("android_client", new LogicJSONBoolean(this._androidClient));

        this._gameObjectManager.Save(logicJSONObject);
        //TODO: LogicCooldownManager::save

        this.SaveShopNewItems(logicJSONObject);

        logicJSONObject.Put("last_league_rank", new LogicJSONNumber(this._lastLeagueRank));
        logicJSONObject.Put("last_league_shuffle", new LogicJSONNumber(this._lastLeagueShuffle));
        logicJSONObject.Put("last_news_seen", new LogicJSONNumber(this._lastSeenNews));
        logicJSONObject.Put("edit_mode_shown", new LogicJSONBoolean(this._editModeShown));

        if (this._troopReqMsg.Length >= 1)
            logicJSONObject.Put("troop_req_msg", new LogicJSONString(this._troopReqMsg));
    }

    private void SaveShopNewItems(LogicJSONObject jsonObject)
    {
        LogicDataTable buildingTable = LogicDataTables.GetTable(LogicDataType.BUILDING);
        LogicDataTable trapTable = LogicDataTables.GetTable(LogicDataType.TRAP);
        LogicDataTable decoTable = LogicDataTables.GetTable(LogicDataType.DECO);

        //TODO
    }
}
