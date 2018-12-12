namespace DiscordBot
{
    interface IEventBroker
    {
        void RegisterHandler(IEventHandler handler);

        void FireEvent(IEvent firedEvent);
    }
}