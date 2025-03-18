using System.Net;
using Wisedev.Magic.Titam.JSON;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.HTTP;

public class ServerHttpClient
{
    private static WebClient CreateWebClient()
    {
        return new WebClient
        {
            Proxy = null
        };
    }

    public static byte[] DownloadBytes(string path)
    {
        try
        {
            using (WebClient client = ServerHttpClient.CreateWebClient())
            {
                return client.DownloadData(string.Format("{0}/{1}", "http://127.0.0.1:8181/supercell", path));
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
                return client.DownloadString(string.Format("{0}/{1}", "http://127.0.0.1:8181/supercell", path));
            }
        }
        catch (Exception)
        {
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
                return LogicJSONParser.ParseObject(client.DownloadString(string.Format("{0}/{1}", "http://127.0.0.1:8181/supercell", path)));
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
                return client.DownloadData(string.Format("{0}/{1}/{2}", "http://127.0.0.1:8181/supercell", resourceSha, path));
            }
        }
        catch (Exception)
        {
            Debugger.Warning(string.Format("ServerHttpClient: file {0} doesn't exist", path));
        }

        return null;
    }
}
