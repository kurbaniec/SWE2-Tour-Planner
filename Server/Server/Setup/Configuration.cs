using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebService_Lib;
using WebService_Lib.Attributes;

namespace Server.Setup
{
    /// <summary>
    /// Reads config and makes it available in the application.
    /// </summary>
    [Component]
    public class Configuration
    {
        public JObject Config { get; }

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
            var configStr = File.ReadAllText(configPath);
            Config = JObject.Parse(configStr);
            
            // Set Port 
            SimpleWebService.Port = (uint) Config["server"]!["port"]!;
        }
    }
}