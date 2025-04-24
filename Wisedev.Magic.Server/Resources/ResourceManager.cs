using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Server.HTTP;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Debug;
using System.Runtime.CompilerServices;

namespace Wisedev.Magic.Server.Resources;

public class ResourceManager
{
    private static DataLoader _loader;

    public static string PATH = Config.AssetsPath;
    public static string LOCAL_RES_PATH = $"{PATH}/local_res";

    public static string? FINGERPRINT_JSON;
    public static string? FINGERPRINT_SHA;
    public static string? FINGERPRINT_VERSION;

    public static string? STARTING_HOME_JSON;

    public static void Init()
    {
        ResourceManager._loader = Config.UseLocalResources
            ? new LocalDataLoader(ResourceManager.LOCAL_RES_PATH)
            : new RemoteDataLoader($"assets");

        ResourceManager.LoadFingerprint();
        ResourceManager.LoadStartingHome();
        ResourceManager.LoadGameResources();
    }

    private static void LoadStartingHome()
    {
        ResourceManager.STARTING_HOME_JSON = ResourceManager.LoadResource("level/starting_home.json");
    }

    private static void LoadFingerprint()
    {
        ResourceManager.FINGERPRINT_JSON = ResourceManager.LoadResource("fingerprint.json");
        if (ResourceManager.FINGERPRINT_JSON != null)
        {
            LogicJSONObject jsonObject = (LogicJSONObject)LogicJSONParser.Parse(ResourceManager.FINGERPRINT_JSON!);
            ResourceManager.FINGERPRINT_SHA = jsonObject.GetJSONString("sha").GetStringValue();
            ResourceManager.FINGERPRINT_VERSION = jsonObject.GetJSONString("version").GetStringValue();

            Debugger.Print($"Sha={ResourceManager.FINGERPRINT_SHA}, version={ResourceManager.FINGERPRINT_VERSION}");
        }
    }

    private static string LoadResource(string path)
    {
        try
        {
            string data = ResourceManager._loader.Load(path);
            Debugger.Print($"{path} has been loaded!");

            return data;
        }
        catch (Exception e)
        {
            Debugger.Error($"Resource ({path}) loading failed: {e.Message}");
            return null;
        }
    }

    private static void LoadGameResources()
    {
        LogicDataTables.Init();
        List<LogicDataTableResource> resources = LogicResources.CreateDataTableResourcesArray();

        for (int i = 0; i < resources.Count; i++)
        {
            string fileName = resources[i].GetFileName();
            LogicResources.Load(resources, i, new Titan.CSV.CSVNode(File.ReadAllLines(fileName), fileName));
        }
    }
}
