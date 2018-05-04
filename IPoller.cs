namespace discordBot
{
    interface IPoller
    {
        void StartPoll(int pollRate);
    }
}