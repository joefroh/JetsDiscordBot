using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLocator;
using Discord.WebSocket;

namespace DiscordBot
{
    public class CommandHandler : IEventHandler
    {
        private Dictionary<string, ICommandExecutor> _commandExecutors;

        public Type Channel { get { return typeof(MessageReceivedEvent); } }

        public CommandHandler()
        {
            _commandExecutors = new Dictionary<string, ICommandExecutor>();
            RegisterCommands();
        }

        /*
            This method uses C# reflection to do some pretty black magic.
            Iterates over all the assemblies in the executable, then pulls out all that implement the ICommandExecutor interface.
            Then it spawns an instance of that type, reads its command string and adds it to the library of executors.

            This is all so I don't have to code and update a gnarly switch of commands in Handle Command. Thank me later.
         */
        private void RegisterCommands()
        {
            if (!Locator.Instance.Fetch<IConfigurationLoader>().Configuration.EnableTextCommands)
            {
                Locator.Instance.Fetch<ILogger>().LogLine("Text Commands disabled in config.");
                return;
            }
            var commands = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(ICommandExecutor).IsAssignableFrom(p) && !p.IsAbstract);
            foreach (var command in commands)
            {
                //TODO Handle conflicts like a good coder.
                var commandExecutor = Activator.CreateInstance(command) as ICommandExecutor;
                _commandExecutors.Add(commandExecutor.CommandString, commandExecutor);
                Locator.Instance.Fetch<ILogger>().LogLine("Registering handler for command: " + commandExecutor.CommandString);
            }
        }

        public async Task HandleCommand(SocketMessage message)
        {
            var command = message.Content.Split(' ');
            if (command[0].StartsWith(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.CommandPrefix))
            {
                ICommandExecutor executor;
                if (command[0].ToLower().Split(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.CommandPrefix)[1] == "help") //help is a privileged command, it needs to know about the others
                {
                    await SendHelpInfo(command, message);
                }
                else if (_commandExecutors.TryGetValue(command[0].ToLower().Split(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.CommandPrefix)[1], out executor))
                {
                    await message.Channel.TriggerTypingAsync();
                    await executor.ExecuteCommand(message);
                }
            }
        }

        private async Task SendHelpInfo(string[] command, SocketMessage message)
        {
            if (command.Count() == 1)
            {
                var builder = new StringBuilder();
                builder.AppendLine("The bot currently supports the following commands:");

                foreach (var ex in _commandExecutors)
                {
                    builder.Append("`" + ex.Value.CommandString + "`" + " ");
                }
                builder.AppendLine();
                builder.AppendLine(string.Format("To get help with any specific command type {0}help <command>", Locator.Instance.Fetch<IConfigurationLoader>().Configuration.CommandPrefix));

                await message.Channel.SendMessageAsync(builder.ToString());
            }
            if (command.Count() > 1)
            {
                var param = command[1].Split(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.CommandPrefix).Last();
                ICommandExecutor executor;
                if (!_commandExecutors.TryGetValue(param, out executor))
                {
                    await message.Channel.SendMessageAsync("Sorry, I don't recognize the command: " + param);
                    return;
                }

                await message.Channel.SendMessageAsync(executor.HelpText);
            }
        }

        public async Task Fire(IEvent firedEvent)
        {
            var payload = firedEvent as MessageReceivedEvent;
            await this.HandleCommand(payload.Message);
        }
    }
}