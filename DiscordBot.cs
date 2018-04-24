using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace discordBot
{
    public class DiscordBot
    {
        private Configuration _config;
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;
        public DiscordBot()
        {
            _config = new Configuration();
            _commandHandler = new CommandHandler(_config);

            var token = _config["Token"];

            if (!String.IsNullOrEmpty(token))
            {
                Console.WriteLine("Got token.");
            }

            Console.WriteLine("Starting up client.");
            _client = new DiscordSocketClient();
            _client.Ready += ConnectedConfirm;
            _client.MessageReceived += MessageReceived;
        }

        #region async tasks
        public async Task LoginAsync()
        {
            Console.WriteLine("Logging in.");
            await _client.LoginAsync(TokenType.Bot, _config["token"]);
        }

        public async Task StartAsync()
        {
            await _client.StartAsync();
        }

        #endregion

        #region event handlers
        private async Task MessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null) return;

            if (message.Author.IsBot != true)
            {
                Console.WriteLine("Non Bot message received.");
                await _commandHandler.HandleCommand(arg);
            }
        }

        private async Task ConnectedConfirm()
        {
            Console.WriteLine("Connected as bot name: " + this._client.CurrentUser.Username);
            foreach (var guild in this._client.Guilds)
            {
                Console.WriteLine("Seeing Guild: " + guild.Name);
                foreach (var channel in guild.TextChannels)
                {
                    Console.WriteLine("Seeing text channel: " + channel.Name);
                    if (channel.Name == "general")
                    {
                        await channel.SendMessageAsync("Connected!");
                    }
                }
            }

            Console.WriteLine("Bot is now ready to interact with users.");
        }
        #endregion
    }
}