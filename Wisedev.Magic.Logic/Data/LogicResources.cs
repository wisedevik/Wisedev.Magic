using Wisedev.Magic.Titam.CSV;
using Wisedev.Magic.Titam.Utils;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Data;

public class LogicResources
{
    public static List<LogicDataTableResource> CreateDataTableResourcesArray()
    {
        List<LogicDataTableResource> arrayList = new List<LogicDataTableResource>(LogicDataTables.TABLE_COUNT);
        arrayList.Add(new LogicDataTableResource("assets/logic/buildings.csv", LogicDataType.BUILDING, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/resources.csv", LogicDataType.RESOURCE, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/characters.csv", LogicDataType.CHARACTER, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/obstacles.csv", LogicDataType.OBSTACLE, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/traps.csv", LogicDataType.TRAP, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/alliance_badges.csv", LogicDataType.ALLIANCE_BADGE, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/globals.csv", LogicDataType.GLOBAL, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/npcs.csv", LogicDataType.NPC, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/decos.csv", LogicDataType.DECO, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/shields.csv", LogicDataType.SHIELD, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/missions.csv", LogicDataType.MISSION, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/heroes.csv", LogicDataType.HERO, 0));
        arrayList.Add(new LogicDataTableResource("assets/logic/leagues.csv", LogicDataType.LEAGUE, 0));

        return arrayList;
    }

    public static void Load(List<LogicDataTableResource> resources, int idx, CSVNode node)
    {
        LogicDataTableResource resource = resources[idx];

        switch (resource.GetTableType())
        {
            case 0:
                LogicDataTables.InitDataTable(node, resource.GetTableIndex());
                break;
            default:
                Debugger.Error("LogicResources.Invalid resource type");
                break;
        }

        if (resources.Count - 1 == idx)
        {
            LogicDataTables.CreateReferences();
        }
    }
}

