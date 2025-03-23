namespace Wisedev.Magic.Titan.Debug;

public interface IDebuggerListener
{
    void HudPrint(string message);
    void Print(string message);
    void Warning(string message);
    void Error(string message);

}
