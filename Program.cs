using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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
            _config = new Configuration();
            var token = _config["token"];

            _client = new DiscordSocketClient();
            _client.Ready += ConnectedConfirm;
            _client.MessageReceived += MessageReceived;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        // event handlers
        // TODO REFACTOR THIS INTO A CLASS
        private async Task MessageReceived(SocketMessage arg)
        {
            if (arg.Author.IsBot != true)
            {
                await arg.Channel.SendMessageAsync("You said: " + arg.Content);
            }
        }

        private async Task ConnectedConfirm()
        {
            var enumerator = this._client.Guilds.GetEnumerator();
            enumerator.MoveNext();

            var channels = enumerator.Current.TextChannels.GetEnumerator();
            while (channels.MoveNext() && channels.Current.Name.ToString() != "general")
            {
                //TODO get this to not be a crap implementation
            }
            var chan = enumerator.Current.GetTextChannel(channels.Current.Id);
            await chan.SendMessageAsync("Connected!");
        }
    }
}
