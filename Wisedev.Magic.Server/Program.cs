using Wisedev.Magic.Logic;
using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Server;
using Wisedev.Magic.Server.Debugging;
using Wisedev.Magic.Server.Network.TCP;
using Wisedev.Magic.Server.Resources;
using Wisedev.Magic.Server.Util;
using Wisedev.Magic.Titan.Debug;

Console.Title = $"Wisedev.Magic | Initializing...";

Debugger.SetListener(new ServerDebuggerListener());
Config.Load();

Console.Title = $"Wisedev.Magic | {Config.Environment}";

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

var exitEvent = new TaskCompletionSource<bool>();

Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    exitEvent.TrySetResult(true);
};

ResourceManager.Init();

TCPGateway tcpGateway = new();
tcpGateway.Start();

await exitEvent.Task;
await tcpGateway.Stop();
