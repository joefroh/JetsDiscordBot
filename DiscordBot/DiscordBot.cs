using ClassLocator;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace discordBot
{
    public class DiscordBot
    {
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;
        private PollHandler _pollHandler;
        private SocketTextChannel _adminChannel;
       

        public DiscordBot()
        {            
            _commandHandler = new CommandHandler();

            var token = Locator.Instance.Fetch<IConfigurationLoader>().Configuration.Token;
            
            if (!String.IsNullOrEmpty(token))
            {
                Locator.Instance.Fetch<ILogger>().LogLine("Got token from config.");
            }

            Locator.Instance.Fetch<ILogger>().LogLine("Starting up client.");
            _client = new DiscordSocketClient();
            Locator.Instance.RegisterInstance<DiscordSocketClient>(_client); //Make the client available downstream as a resource
            _client.Ready += GatewayHandshook;
            _client.MessageReceived += MessageReceived;
            _pollHandler = new PollHandler();
        }


        #region async tasks
        public async Task LoginAsync()
        {
            Locator.Instance.Fetch<ILogger>().LogLine("Logging in.");
            await _client.LoginAsync(TokenType.Bot, Locator.Instance.Fetch<IConfigurationLoader>().Configuration.Token);
            Locator.Instance.Fetch<ILogger>().LogLine("Successfully logged in.");
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
                await _commandHandler.HandleCommand(arg);
            }
        }

        private async Task GatewayHandshook()
        {
            Locator.Instance.Fetch<ILogger>().LogLine("Connected as bot name: " + this._client.CurrentUser.Username);
            foreach (var guild in this._client.Guilds)
            {
                Locator.Instance.Fetch<ILogger>().LogLine("Seeing Guild: " + guild.Name);
                foreach (var channel in guild.TextChannels)
                {
                    Locator.Instance.Fetch<ILogger>().LogLine("Seeing text channel: " + channel.Name);
                }
            }
            var adminGuild = _client.GetGuild(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.AdminServerID);
            if (adminGuild != null)
            {
                _adminChannel = adminGuild.GetTextChannel(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.AdminChannelID);
            }

            await _adminChannel.SendMessageAsync("Bot has connected.");
            Locator.Instance.Fetch<ILogger>().LogLine("Bot is now ready to interact with users.");

            _pollHandler.StartPollers();
        }
        #endregion
    }
}