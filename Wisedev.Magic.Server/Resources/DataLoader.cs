namespace Wisedev.Magic.Server.Resources;

public abstract class DataLoader
{
    public abstract string Load(string path);

    protected void ValidatePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be empty", nameof(path));
    }
}
