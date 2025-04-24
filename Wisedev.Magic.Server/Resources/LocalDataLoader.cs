namespace Wisedev.Magic.Server.Resources;

public class LocalDataLoader : DataLoader
{
    private readonly string _basePath;

    public LocalDataLoader(string basePath)
    {
        _basePath = basePath;
    }

    public override string Load(string path)
    {
        this.ValidatePath(path);
        string fullPath = Path.Combine(_basePath, path);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Local resource not found at: {fullPath}");

        return File.ReadAllText(fullPath);
    }
}
