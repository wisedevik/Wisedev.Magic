using System.Text.Json;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server;

public static class Config
{
    public static string ApiUrl { get; private set; }
    public static string ApiToken { get; private set; }
    public static string MongoConnectionString { get; private set; }
    public static string DatabaseName { get; private set; }
    public static int ServerPort { get; private set; }
    public static string AssetsPath { get; private set; }
    public static string Environment { get; private set; }

    public static void Load(string path = "config.json")
    {
        if (!File.Exists(path))
        {
            Debugger.Error($"Configuration file {path} not found!");
            System.Environment.Exit(1);
        }

        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<ConfigModel>(json);

        ValidateConfig(config);

        ApiUrl = config.ApiUrl;
        ApiToken = config.ApiToken;
        MongoConnectionString = config.MongoConnectionString;
        DatabaseName = config.DatabaseName;
        ServerPort = config.ServerPort;
        AssetsPath = config.AssetsPath;
        Environment = config.Environment;
    }

    private static void ValidateConfig(ConfigModel config)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(config.ApiUrl))
            errors.Add("ApiUrl is required");

        if (string.IsNullOrWhiteSpace(config.ApiToken))
            errors.Add("ApiToken is required");

        if (errors.Count > 0)
        {
            Debugger.Error("Configuration errors:\n" + string.Join("\n", errors));
            System.Environment.Exit(1);
        }
    }

    private class ConfigModel
    {
        public string ApiUrl { get; set; }
        public string ApiToken { get; set; }
        public string MongoConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public int ServerPort { get; set; }
        public string AssetsPath { get; set; }
        public string Environment { get; set; }
    }
}
