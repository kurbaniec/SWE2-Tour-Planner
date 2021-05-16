using System;
using System.IO;
using Client.Utils.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Client.Logic.Setup
{
    /// <summary>
    /// Reads config and makes it available in the application.
    /// </summary>
    public class Configuration
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public JObject Config { get; }
        public string BaseUrl { get; }

        private readonly ILogger logger = ApplicationLogging.CreateLogger<Configuration>();

        public Configuration()
        {
            // Check for config in directory where executable is located
            string runningPath = AppDomain.CurrentDomain.BaseDirectory!;
            string configPath = $"{runningPath}{Path.DirectorySeparatorChar}config.json";
            if (!File.Exists(configPath))
            {
                // Check for config in project structure
                configPath =
                    Path.GetFullPath(
                        Path.Combine(
                            runningPath,
                            ".." + Path.DirectorySeparatorChar +
                            ".." + Path.DirectorySeparatorChar +
                            ".." + Path.DirectorySeparatorChar +
                            ".." + Path.DirectorySeparatorChar +
                            ".." + Path.DirectorySeparatorChar + "config.json"));
                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException("Could not find config file");
                }
            }

            // Parse config
            var configStr = File.ReadAllText(configPath);
            Config = JObject.Parse(configStr);
            // Set often used config values as properties
            BaseUrl = (string) Config["client"]!["base-url"]!;
            // Set Logging
            // See: https://github.com/huorswords/Microsoft.Extensions.Logging.Log4Net.AspNetCore
            var loggingConfig = (string) Config["client"]!["logger-config"]!;
            ApplicationLogging.LoggerFactory.AddLog4Net(loggingConfig);
            logger.Log(LogLevel.Information, "Configuration found, read and initialized");
        }
    }
}