using Wisedev.Magic.Server.HTTP;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Resources;

public class RemoteDataLoader : DataLoader
{
    private readonly string _baseUrl;

    public RemoteDataLoader(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public override string Load(string path)
    {
        ValidatePath(path);
        string url = $"{_baseUrl}/{path}";
        var data = ServerHttpClient.DownloadString(url);

        return data ?? throw new Exception($"Failed to download resource from {url}");
    }
}
