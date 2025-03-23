namespace Wisedev.Magic.Server.Util;

public static class EnvironmentUtil
{
    public static string GetEnvironmentAbbreviation(string environment)
    {
        return environment switch
        {
            "production" => "prod",
            "integration" => "int",
            _ => environment.Substring(0, Math.Min(environment.Length, 5))
        };
    }
}
