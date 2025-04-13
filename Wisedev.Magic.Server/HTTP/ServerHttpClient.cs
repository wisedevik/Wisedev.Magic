using System.Net;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.HTTP;

public class ServerHttpClient
{
    public static string URL = "https://api.bladewise.xyz/supercell";
    private static string TOKEN = Config.ApiToken;

    private static WebClient CreateWebClient()
    {
        WebClient client = new WebClient
        {
            Proxy = null
        };
        if (Config.ApiShield)
            client.Headers["Authorization"] = $"Bearer {ServerHttpClient.TOKEN}";

        return client;

    }

    public static byte[] DownloadBytes(string path)
    {
        try
        {
            using (WebClient client = ServerHttpClient.CreateWebClient())
            {
                return client.DownloadData(string.Format("{0}/{1}", ServerHttpClient.URL, path));
            }
        }
        catch (Exception)
        {
            Debugger.Warning(string.Format("ServerHttpClient: file {0} doesn't exist", path));
        }

        return null;
    }

    public static string DownloadString(string path)
    {
        try
        {
            using (WebClient client = ServerHttpClient.CreateWebClient())
            {
                return client.DownloadString(string.Format("{0}/{1}", ServerHttpClient.URL, path));
            }
        }
        catch (Exception ex)
        {
            Debugger.Error($"{ex.Message}\n{ex.StackTrace}");
            Debugger.Warning(string.Format("ServerHttpClient: file {0} doesn't exist", path));
        }

        return null;
    }

    public static LogicJSONObject DownloadJSON(string path)
    {
        try
        {
            using (WebClient client = ServerHttpClient.CreateWebClient())
            {
                return LogicJSONParser.ParseObject(client.DownloadString(string.Format("{0}/{1}", ServerHttpClient.URL, path)));
            }
        }
        catch (Exception)
        {
            Debugger.Warning(string.Format("ServerHttpClient: file {0} doesn't exist", path));
        }

        return null;
    }

    public static byte[] DownloadAsset(string resourceSha, string path)
    {
        try
        {
            using (WebClient client = ServerHttpClient.CreateWebClient())
            {
                return client.DownloadData(string.Format("{0}/{1}/{2}", ServerHttpClient.URL, resourceSha, path));
            }
        }
        catch (Exception)
        {
            Debugger.Warning(string.Format("ServerHttpClient: file {0} doesn't exist", path));
        }

        return null;
    }
}
