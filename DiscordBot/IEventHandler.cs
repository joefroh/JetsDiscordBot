namespace discordBot {
    public interface IEventHandler {
        string ChannelString { get; }

        void Fire (IEvent firedEvent);
    }
}