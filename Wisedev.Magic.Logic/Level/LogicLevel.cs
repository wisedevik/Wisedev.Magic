using Wisedev.Magic.Logic.Avatar;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Home;
using Wisedev.Magic.Logic.Mode;
using Wisedev.Magic.Logic.Time;
using Wisedev.Magic.Titam.Utils;

namespace Wisedev.Magic.Logic.Level;

public class LogicLevel
{
    private LogicGameListener _gameListener;

    private LogicGameMode _gameMode;
    private LogicTime _logicTime;

    private int _lastSeenNews;

    private LogicAvatar _visitorAvatar;
    private LogicAvatar _homeOwnerAvatar;

    private LogicClientHome _home;

    private LogicArrayList<int> _newShopBuildings;
    private LogicArrayList<int> _newShopTraps;
    private LogicArrayList<int> _newShopDecos;

    public LogicLevel(LogicGameMode gameMode)
    {
        _gameMode = gameMode;
        this._logicTime = new LogicTime();

        this._newShopBuildings = new LogicArrayList<int>();
        this._newShopTraps = new LogicArrayList<int>();
        this._newShopDecos = new LogicArrayList<int>();

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
    }

    public void SetHomeOwnerAvatar(LogicAvatar avatar)
    {
        this._homeOwnerAvatar = avatar;

        // TODO: more logic
    }

    public void Tick()
    {

    }

    public void SubTick()
    {

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
}
