using System.Threading.Tasks;
using Discord.WebSocket;

namespace discordBot
{
    internal interface IReactionActor
    {
        string TriggerString { get; }
        Task React(SocketMessage msg);
    }
}