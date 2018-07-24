using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace discordBot
{
    public class CommandHandler
    {
        private Configuration _config;
        private Dictionary<string, ICommandExecutor> _commandExecutors;
        public CommandHandler(Configuration config)
        {
            _config = config;
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
            if (!_config.EnableTextCommands)
            {
                Console.WriteLine("Text Commands disabled in config.");
                return;
            }
            var commands = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(ICommandExecutor).IsAssignableFrom(p) && !p.IsAbstract);
            foreach (var command in commands)
            {
                //TODO Handle conflicts like a good coder.
                var commandExecutor = Activator.CreateInstance(command, _config) as ICommandExecutor;
                _commandExecutors.Add(commandExecutor.CommandString, commandExecutor);
                Console.WriteLine("Registering handler for command: " + commandExecutor.CommandString);
            }
        }

        public async Task HandleCommand(SocketMessage message)
        {
            var command = message.Content.Split(' ');
            if (command[0].StartsWith(_config.CommandPrefix))
            {
                ICommandExecutor executor;
                if (_commandExecutors.TryGetValue(command[0].ToLower().Split(_config.CommandPrefix)[1], out executor))
                {
                    await executor.ExecuteCommand(message);
                }
            }
        }
    }
}