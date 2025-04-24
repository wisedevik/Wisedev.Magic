using System.Collections.Immutable;
using System.Reflection;
using Wisedev.Magic.Titan.Message;

namespace Wisedev.Magic.Logic.Message;

public class LogicMagicMessageFactory : LogicMessageFactory
{
    public const string RC4_KEY = "fhsd6f86f67rt8fw78fw789we78r9789wer6re";

    private ImmutableDictionary<int, Type> _messages;

    public static readonly LogicMessageFactory Instance;

    static LogicMagicMessageFactory()
    {
        Instance = new LogicMagicMessageFactory();
    }

    private LogicMagicMessageFactory()
    {
        _messages = CreateMessageMap();
    }

    private ImmutableDictionary<int, Type> CreateMessageMap()
    {
        var builder = ImmutableDictionary.CreateBuilder<int, Type>();

        IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttribute<PiranhaMessageAttribute>() != null);

        foreach (Type type in types)
        {
            PiranhaMessageAttribute messageAttribute = type.GetCustomAttribute<PiranhaMessageAttribute>()!;

            if (!builder.TryAdd(messageAttribute.MessageType, type))
                throw new Exception($"Piranha message with type {messageAttribute.MessageType} defined twice!");
        }

        return builder.ToImmutable();
    }

    public override PiranhaMessage? CreateMessageByType(int messageType)
    {
        return this._messages.TryGetValue(messageType, out Type? type) ?
            Activator.CreateInstance(type) as PiranhaMessage : null;
    }
}
