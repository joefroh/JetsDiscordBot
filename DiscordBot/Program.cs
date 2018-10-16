using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClassLocator;

namespace discordBot
{
    class Program
    {
        // main
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        // private members
        private DiscordBot _bot;

        // async main
        public async Task MainAsync()
        {
            Locator.Instance.Fetch<ILogger>().LogLine("Starting up the bot.");
            _bot = new DiscordBot();
            await _bot.LoginAsync();
            await _bot.StartAsync();
            Locator.Instance.Fetch<ILogger>().LogLine("Awaiting main thread.");
            await Task.Delay(-1);
        }
    }
}
