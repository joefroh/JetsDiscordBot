using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            Console.WriteLine("Starting up the bot.");
            _bot = new DiscordBot();
            await _bot.LoginAsync();
            await _bot.StartAsync();
            Console.WriteLine("Awaiting.");
            await Task.Delay(-1);
        }
    }
}
