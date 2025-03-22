using Wisedev.Magic.Server.HTTP;
using Wisedev.Magic.Titam.JSON;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Resources;

class ResourceManager
{
    public const string PATH = "assets";

    public static string? FINGERPRINT_JSON;
    public static string? FINGERPRINT_SHA;
    public static string? FINGERPRINT_VERSION;

    public static string? STARTING_HOME_JSON;

    public static void Init()
    {
        ResourceManager.LoadFingerprint();
        ResourceManager.LoadStartingHome();
    }

    private static void LoadStartingHome()
    {
        ResourceManager.STARTING_HOME_JSON = ServerHttpClient.DownloadString($"{ResourceManager.PATH}/level/starting_home.json");
        if (ResourceManager.STARTING_HOME_JSON == null)
        {
            Debugger.Error("ResourceManager.LoadStartingHome: Unable to download the starting_home.json!");
            return;
        }

        Debugger.Print("Starting home has been downloaded!");
    }

    private static void LoadFingerprint()
    {
        ResourceManager.FINGERPRINT_JSON = ServerHttpClient.DownloadString($"{ResourceManager.PATH}/fingerprint.json");
        if (ResourceManager.FINGERPRINT_JSON != null )
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
