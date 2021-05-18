using System;
namespace DiscordBot
{
    public interface IEvent
    {
        Type Channel { get; }
    }
}