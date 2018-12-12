using System.Threading.Tasks;

namespace DiscordBot
{
    interface IEventBroker
    {
        void RegisterHandler(IEventHandler handler);

        Task FireEvent(IEvent firedEvent);
    }
}