using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using Wisedev.Magic.Logic.Command.Listener;
using Wisedev.Magic.Logic.Level;
using Wisedev.Magic.Logic.Message;
using Wisedev.Magic.Titan.DataStream;
using Wisedev.Magic.Titan.JSON;
using Wisedev.Magic.Titan.Message;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Logic.Command;

public class LogicCommandManager
{
    private LogicLevel _level;
    private LogicCommandManagerListener _listener;
    private List<LogicCommand> _commands;

    private static ImmutableDictionary<int, Type> _commandsMap;

    public LogicCommandManager(LogicLevel level)
    {
        this._level = level;
        this._commands = new List<LogicCommand>();
    }

    static LogicCommandManager()
    {
        LogicCommandManager._commandsMap = CreateCommandsMap();
    }

    private static ImmutableDictionary<int, Type> CreateCommandsMap()
    {
        var builder = ImmutableDictionary.CreateBuilder<int, Type>();

        IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttribute<LogicCommandAttribute>() != null);

        foreach (Type type in types)
        {
            LogicCommandAttribute messageAttribute = type.GetCustomAttribute<LogicCommandAttribute>()!;

            if (!builder.TryAdd(messageAttribute.CommandType, type))
                throw new Exception($"Piranha message with type {messageAttribute.CommandType} defined twice!");
        }

        return builder.ToImmutable();
    }

    public void SetListener(LogicCommandManagerListener listener)
    {
        this._listener = listener;
    }

    public void AddCommand(LogicCommand command)
    {
        if (command != null)
        {
            if (this._level.GetState() == 4)
            {
                command.Destruct();
                command = null;
            }
            else
            {
                this._commands.Add(command);
            }
        }
    }

    public bool IsCommandAllowedInCurrentState(LogicCommand command)
    {
        int commandType = command.GetCommandType();
        int state = this._level.GetState();

        if (state == 4)
        {
            Debugger.Warning("Execute command failed! Commands are not allowed in visit state.");
            return false;
        }

        if (commandType <= 1000)
        {
            if (commandType >= 500 && commandType < 600 && state != 1)
            {
                Debugger.Warning("Execute command failed! Command is only allowed in home state");
                return false;
            }

            if (commandType >= 600 && commandType < 700 & state != 2 && state != 5)
            {
                Debugger.Warning("Execute command failed! Command is only allowed in attack state.");
                return false;
            }
        }

        return true;
    }

    public void SubTick()
    {
        int subTick = this._level.GetLogicTime().GetTick();

        for (int i = 0; i < this._commands.Count; i++)
        {
            LogicCommand command = this._commands[i];
            if (command.GetExecuteSubTick() < subTick)
            {
                Debugger.Error($"Execute command failed! Command should have been executed already. (type={command.GetCommandType()} server_tick={subTick} command_tick={command.GetExecuteSubTick()})");
            }

            if (command.GetExecuteSubTick() == subTick)
            {
                if (this.IsCommandAllowedInCurrentState(command))
                {
                    if (command.Execute(this._level) == 0)
                    {
                        //this._listener.CommandExecuted(command);
                    }

                    this._commands.RemoveAt(i--);
                }
                else
                {
                    Debugger.Warning($"Execute command failed! Command not allowed in current state. (type={command.GetCommandType()} current_state={this._level.GetState()}");
                }
            }
        }
    }

    public static LogicCommand? CreateCommand(int commandType)
    {
        LogicCommand? command = _commandsMap.TryGetValue(commandType, out Type? type) ?
            Activator.CreateInstance(type) as LogicCommand : null;

        if (command != null)
            Debugger.Print($"LogicCommandManager.CreateCommand created command with type={commandType}");

        return command;
    }

    public static void EncodeCommand(ChecksumEncoder encoder, LogicCommand command)
    {
        encoder.WriteInt(command.GetCommandType());
        command.Encode(encoder);
    }

    public static LogicCommand? DecodeCommand(ByteStream stream)
    {
        int commandType = stream.ReadInt();
        LogicCommand? command = LogicCommandManager.CreateCommand(commandType);

        if (command == null)
        {
            Debugger.Warning($"LogicCommandManager.DecodeCommand - command {commandType} is NULL!");
        }
        else
        {
            command.Decode(stream);
        }

        return command;
    }

    public static void SaveCommandToJSON(LogicJSONObject jsonObject, LogicCommand command)
    {
        jsonObject.Put("ct", new LogicJSONNumber(command.GetCommandType()));
        jsonObject.Put("c", command.GetJSONForReplay());
    } 
}
