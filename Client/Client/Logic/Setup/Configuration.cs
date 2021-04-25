using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Client.Logic.Setup
{
    /// <summary>
    /// Reads config and makes it available in the application.
    /// </summary>
    public class Configuration
    {
        public JObject Config { get; }
        
        public string BaseUrl { get; }

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
        }
    }
}