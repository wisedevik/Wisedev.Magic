using System;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server.Debugging
{
    internal class ServerDebuggerListener : IDebuggerListener
    {
        public void Error(string log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] >> {log}", ConsoleColor.Red);
            Console.ResetColor();
        }

        public void HudPrint(string log)
        {
            Console.WriteLine($"[HUD] >> {log}");
        }

        public void Print(string log)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[LOG] >> {log}");
            Console.ResetColor();
        }

        public void Warning(string log)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARNING] >> {log}", ConsoleColor.Yellow);
            Console.ResetColor();
        }
    }
}
