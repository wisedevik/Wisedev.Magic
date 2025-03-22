using Wisedev.Magic.Titam.JSON;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Resources;

class ResourceManager
{
    public static string FINGERPRINT_PATH = "Assets/fingerprint.json";

    public static string FINGERPRINT_JSON;
    public static string FINGERPRINT_SHA;
    public static string FINGERPRINT_VERSION;

    public static string STARTING_HOME_JSON;

    public static void Init()
    {
        ResourceManager.LoadFingerprint();
        ResourceManager.LoadStartingHome();
    }

    private static void LoadStartingHome()
    {
        ResourceManager.STARTING_HOME_JSON = File.ReadAllText("Assets/level/starting_home.json");
        if (ResourceManager.STARTING_HOME_JSON == null)
        {
            Debugger.Error("ResourceManager.LoadStartingHome: starting_home.json not exist");
            return;
        }

        Debugger.Print("Starting home is loaded!");
    }

    private static void LoadFingerprint()
    {
        ResourceManager.FINGERPRINT_JSON = File.ReadAllText(ResourceManager.FINGERPRINT_PATH);

        if (ResourceManager.FINGERPRINT_JSON != null )
        {
            LogicJSONObject jsonObject = (LogicJSONObject)LogicJSONParser.Parse(ResourceManager.FINGERPRINT_JSON!);

            ResourceManager.FINGERPRINT_SHA = jsonObject.GetJSONString("sha").GetStringValue();
            ResourceManager.FINGERPRINT_VERSION = jsonObject.GetJSONString("version").GetStringValue();
        }
        else
        {
            Debugger.Error($"ResourceManager.LoadFingerprint: {ResourceManager.FINGERPRINT_PATH} not exist");
            return;
        }

        Debugger.Print($"Fingerprint is loaded! server sha={ResourceManager.FINGERPRINT_SHA} server fingerprint version={ResourceManager.FINGERPRINT_VERSION}");
    }
}
