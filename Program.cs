using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
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
        private DiscordSocketClient _client;
        private Configuration _config;

        // async main
        public async Task MainAsync()
        {
            Console.WriteLine("Starting up the bot.");
            _config = new Configuration();
            var token = _config["token"];

            if (!String.IsNullOrEmpty(token))
            {
                Console.WriteLine("Got token.");
            }

            Console.WriteLine("Starting up client.");
            _client = new DiscordSocketClient();
            _client.Ready += ConnectedConfirm;
            _client.MessageReceived += MessageReceived;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            Console.WriteLine("Awaiting.");
            await Task.Delay(-1);
        }

        private Task Connected()
        {
            throw new NotImplementedException();
        }

        // event handlers
        // TODO REFACTOR THIS INTO A CLASS
        private async Task MessageReceived(SocketMessage arg)
        {
            if (arg.Author.IsBot != true)
            {
                Console.WriteLine("Non Bot message received.");
                await arg.Channel.SendMessageAsync("You said: " + arg.Content);
            }
        }

        private async Task ConnectedConfirm()
        {
            Console.WriteLine("Connected as bot name: " + this._client.CurrentUser.Username);
            foreach (var guild in this._client.Guilds)
            {
                Console.WriteLine("Seeing Guild: " + guild.Name);
            }

            var enumerator = this._client.Guilds.GetEnumerator();
            enumerator.MoveNext();

            var channels = enumerator.Current.TextChannels.GetEnumerator();
            while (channels.MoveNext() && channels.Current.Name.ToString() != "general")
            {
                //TODO get this to not be a crap implementation
            }
            var chan = enumerator.Current.GetTextChannel(channels.Current.Id);
            await chan.SendMessageAsync("Connected!");
            Console.WriteLine("Bot is now ready to interact with users.");
        }
    }
}
