using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public interface IEventHandler
    {
        Type Channel { get; }

        Task Fire(IEvent firedEvent);
    }
}