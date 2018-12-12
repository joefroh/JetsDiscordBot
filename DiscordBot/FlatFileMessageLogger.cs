using System;
using System.IO;
using ClassLocator;
using Discord.WebSocket;

namespace discordBot
{
    public class FlatFileMessageLogger : IMessageLogger
    {
        StreamWriter writer;
        public FlatFileMessageLogger()
        {
            // Open a stream writer, if the file exists append = true
            writer = new StreamWriter(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.MessageLogFile, true);
            writer.AutoFlush = true;
        }

        public void LogMessage(SocketMessage message)
        {
            // {timestamp}/t{server}/t{channel}/t{message}/t{user}
            var output = String.Format("{0}\t{1}\t{2}\t{3}\t{4}", message.Timestamp, (message.Channel as SocketGuildChannel).Guild, message.Channel, message.Content, message.Author);
            writer.WriteLineAsync(output);
        }
    }
}