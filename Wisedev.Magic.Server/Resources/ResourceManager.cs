using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Server.HTTP;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Resources;

class ResourceManager
{
    public static string PATH = Config.AssetsPath;
    public static string LOCAL_RES_PATH = $"{PATH}/local_res";

    public static string? FINGERPRINT_JSON;
    public static string? FINGERPRINT_SHA;
    public static string? FINGERPRINT_VERSION;

    public static string? STARTING_HOME_JSON;

    public static void Init()
    {
        ResourceManager.LoadFingerprint();
        ResourceManager.LoadStartingHome();
        ResourceManager.LoadResources();
    }

    private static void LoadStartingHome()
    {
        if (Config.UseLocalResources)
        {
            if (File.Exists($"{LOCAL_RES_PATH}/level/starting_home.json"))
            {
                ResourceManager.STARTING_HOME_JSON = File.ReadAllText($"{LOCAL_RES_PATH}/level/starting_home.json");
                Debugger.Print("Starting home has been loaded from local resources!");
            }
            else
            {
                Debugger.Error($"ResourceManager.LoadStartingHome: Unable to load the starting_home.json from local resources!");
                return;
            }
        }
        else
        {
            ResourceManager.STARTING_HOME_JSON = ServerHttpClient.DownloadString($"{ResourceManager.PATH}/level/starting_home.json");
            if (ResourceManager.STARTING_HOME_JSON == null)
            {
                Debugger.Error("ResourceManager.LoadStartingHome: Unable to download the starting_home.json!");
                return;
            }

            Debugger.Print("Starting home has been downloaded!");
        }

    }

    private static void LoadFingerprint()
    {
        if (Config.UseLocalResources)
        {
            if (File.Exists($"{LOCAL_RES_PATH}/fingerprint.json"))
            {
                ResourceManager.FINGERPRINT_JSON = File.ReadAllText($"{LOCAL_RES_PATH}/fingerprint.json");
                if (ResourceManager.FINGERPRINT_JSON != null)
                {
                    LogicJSONObject jsonObject = (LogicJSONObject)LogicJSONParser.Parse(ResourceManager.FINGERPRINT_JSON!);

                    ResourceManager.FINGERPRINT_SHA = jsonObject.GetJSONString("sha").GetStringValue();
                    ResourceManager.FINGERPRINT_VERSION = jsonObject.GetJSONString("version").GetStringValue();
                }
                Debugger.Print($"Fingerprint(server_sha={ResourceManager.FINGERPRINT_SHA}, v={ResourceManager.FINGERPRINT_VERSION}) has been loaded from local resources!");
            }
            else
            {
                Debugger.Error($"ResourceManager.LoadFingerprint: Unable to load the fingerprint.json from local resources!");
                return;
            }
        }
        else
        {
            ResourceManager.FINGERPRINT_JSON = ServerHttpClient.DownloadString($"{ResourceManager.PATH}/fingerprint.json");
            if (ResourceManager.FINGERPRINT_JSON != null)
            {
                LogicJSONObject jsonObject = (LogicJSONObject)LogicJSONParser.Parse(ResourceManager.FINGERPRINT_JSON!);

                ResourceManager.FINGERPRINT_SHA = jsonObject.GetJSONString("sha").GetStringValue();
                ResourceManager.FINGERPRINT_VERSION = jsonObject.GetJSONString("version").GetStringValue();
            }
            else
            {
                Debugger.Error($"ResourceManager.LoadFingerprint: Unable to download the fingerprint.json!");
                return;
            }

            Debugger.Print($"Fingerprint has been downloaded! server sha={ResourceManager.FINGERPRINT_SHA} server fingerprint version={ResourceManager.FINGERPRINT_VERSION}");
        }


    }

    private static void LoadResources()
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
