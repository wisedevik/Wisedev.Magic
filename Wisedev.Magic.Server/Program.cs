using Wisedev.Magic.Server.Debugging;
using Wisedev.Magic.Server.HTTP;
using Wisedev.Magic.Server.Network.TCP;
using Wisedev.Magic.Titam.DataStream;
using Wisedev.Magic.Titam.JSON;
using Wisedev.Magic.Titan.Debug;

Console.Title = $"Wisedev.Magic | integration | 5.2.1";

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

Debugger.SetListener(new ServerDebuggerListener());

TCPGateway tcpGateway = new();
tcpGateway.Start();

await exitEvent.Task;
await tcpGateway.Stop();
