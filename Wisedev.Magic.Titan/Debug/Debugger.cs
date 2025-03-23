namespace Wisedev.Magic.Titan.Debug;

public static class Debugger
{
    private static IDebuggerListener? _listener;

    public static void Print(string log)
    {
        _listener?.Print(log);
    }

    public static void Warning(string log)
    {
        _listener?.Warning(log);
    }

    public static void Error(string log)
    {
        _listener?.Error(log);
        throw new Exception(log);
    }

    public static void HudPrint(string log)
    {
        _listener?.HudPrint(log);
    }

    public static bool DoAssert(bool condition, string message)
    {
        if (!condition)
            Error(message);

        return condition;
    }

    public static void SetListener(IDebuggerListener listener)
    {
        _listener = listener;
    }
}