using System;

namespace DiscordBot
{
    public interface IEventHandler
    {
        Type Channel { get; }

        void Fire(IEvent firedEvent);
    }
}