using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Server.Debugging;
using Wisedev.Magic.Server.Network.TCP;
using Wisedev.Magic.Server.Resources;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Server;

class Program
{
    private static readonly TaskCompletionSource<bool> _exitEvent = new TaskCompletionSource<bool>();

    private static async Task Main()
    {
        Program.InitializeConsole();
        Program.InitializeDebugger();
        Program.LoadConfiguration();
        Program.DisplayBanner();

        Debugger.Print("Starting services...\n");
        ResourceManager.Init();

        TCPGateway tcpGateway = new();
        tcpGateway.Start();

        await Program.WaitForExit();

        await tcpGateway.Stop();
    }

    private static void InitializeConsole()
    {
        Console.Title = "Wisedev.Magic | Initialazing...";
        Console.CancelKeyPress += OnCancelKeyPress;
    }

    private static void InitializeDebugger()
    {
        Debugger.SetListener(new ServerDebuggerListener());
    }

    private static void LoadConfiguration()
    {
        Config.Load();
        Console.Title = $"Wisedev.Magic | {Config.Environment}";
    }

    private static void DisplayBanner()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("""
    
     ███▄ ▄███▓▄▄▄       ▄████ ██▓▄████▄  
    ▓██▒▀█▀ ██▒████▄    ██▒ ▀█▓██▒██▀ ▀█  
    ▓██    ▓██▒██  ▀█▄ ▒██░▄▄▄▒██▒▓█    ▄ 
    ▒██    ▒██░██▄▄▄▄██░▓█  ██░██▒▓▓▄ ▄██▒
    ▒██▒   ░██▒▓█   ▓██░▒▓███▀░██▒ ▓███▀ ░
    ░ ▒░   ░  ░▒▒   ▓▒█░░▒   ▒░▓ ░ ░▒ ▒  ░
    ░  ░      ░ ▒   ▒▒ ░ ░   ░ ▒ ░ ░  ▒   
    ░      ░    ░   ▒  ░ ░   ░ ▒ ░        
           ░        ░  ░     ░ ░ ░ ░      
                                 ░        
    
    """);
        Console.ResetColor();
        Console.WriteLine("Copyright WiseDev 2025-2026. All rights reserved.\n");
    }

    private static async Task WaitForExit()
    {
        await Program._exitEvent.Task;
    }

    private static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs eventArgs)
    {
        eventArgs.Cancel = true;
        Program._exitEvent.TrySetResult(true);
    }
}
