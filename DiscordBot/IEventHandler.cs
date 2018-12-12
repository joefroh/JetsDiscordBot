namespace DiscordBot {
    public interface IEventHandler {
        string ChannelString { get; }

        void Fire (IEvent firedEvent);
    }
}