using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace discordBot
{
    public class DiscordBot
    {
        private Configuration _config;
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;
        private List<RedditClient> _subreddits;
        private SocketTextChannel _adminChannel;

        public DiscordBot()
        {
            _config = ConfigurationLoader.LoadConfiguration();
            _commandHandler = new CommandHandler(_config);
            _subreddits = new List<RedditClient>();

            PopulateSubreddits();

            var token = _config.Token;

            if (!String.IsNullOrEmpty(token))
            {
                Console.WriteLine("Got token.");
            }

            Console.WriteLine("Starting up client.");
            _client = new DiscordSocketClient();
            _client.Ready += ConnectedConfirm;
            _client.MessageReceived += MessageReceived;

            Task.Run(() => RedditPoll());
        }

        private void PopulateSubreddits()
        {
            foreach (var subreddit in _config.SubredditConfig)
            {
                _subreddits.Add(new RedditClient(subreddit));
            }
        }

        #region async tasks
        public async Task LoginAsync()
        {
            Console.WriteLine("Logging in.");
            await _client.LoginAsync(TokenType.Bot, _config.Token);
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
                }
            }
            var adminGuild = _client.GetGuild(_config.AdminServerID);
            if (adminGuild != null)
            {
                _adminChannel = adminGuild.GetTextChannel(_config.AdminChannelID);
            }

            await _adminChannel.SendMessageAsync("Bot has connected.");
            Console.WriteLine("Bot is now ready to interact with users.");
        }
        #endregion

        private void RedditPoll()
        {
            while (true)
            {
                foreach (var subreddit in _subreddits)
                {
                    var guild = _client.GetGuild(subreddit.TargetServer);
                    if (null == guild) continue;

                    var channel = guild.GetTextChannel(subreddit.TargetChannel);
                    if (null == channel) continue;

                    var newSubmissions = subreddit.UpdateReddit();
                    foreach (var sub in newSubmissions)
                    {
                        channel.SendMessageAsync(sub);
                    }
                }
                Thread.Sleep(_config.RedditRefreshTimer * 60000);
            }
        }
    }
}