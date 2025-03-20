using Wisedev.Magic.Titam.JSON;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Resources;

class ResourceManager
{
    public static string FINGERPRINT_PATH = "Assets/fingerprint.json";

    public static string FINGERPRINT_JSON;
    public static string FINGERPRINT_SHA;
    public static string FINGERPRINT_VERSION;

    public static void Init()
    {
        ResourceManager.LoadFingerprint();
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
