using System;
using System.IO;
using ClassLocator;

namespace DiscordBot
{
    public class Logger : ILogger
    {
        StreamWriter logFile = null;
        public Logger()
        {
            var logFileName = Locator.Instance.Fetch<IConfigurationLoader>().Configuration.LogFile;
            if (logFileName == "")
            {
                Console.WriteLine("No log file path found. All logs will be outputted to the Console.");
                return;
            }

            logFile = new StreamWriter(logFileName);
            logFile.AutoFlush = true; // Needed to ensure the log doesn't lose things if we crash
        }

        public void LogLine(string line)
        {
            var output = String.Format("{0}: {1}", DateTime.UtcNow.ToString(), line);
            if (logFile == null)
            {
                Console.WriteLine(output);
                return;
            }
            logFile.WriteLine(output);
        }
    }
}