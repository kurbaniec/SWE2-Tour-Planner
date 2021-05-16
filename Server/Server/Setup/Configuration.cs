using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WebService_Lib;
using WebService_Lib.Attributes;
using WebService_Lib.Logging;

namespace Server.Setup
{
    /// <summary>
    /// Reads config and makes it available in the application.
    /// </summary>
    [Component]
    public class Configuration
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public JObject Config { get; }
        public string MapApiKey { get; }
        public string RoutePath { get; }
        public string ExportPath { get; }
        public string PostgresConnString { get; }

        private readonly ILogger logger = WebServiceLogging.CreateLogger<Configuration>();

        public Configuration()
        {
            // Check for config in directory where executable is located
            string runningPath = AppDomain.CurrentDomain.BaseDirectory!;
            string configPath = $"{runningPath}config.json";
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
            // Set Logging
            var loggingConfig = (string) Config["server"]!["logger-config"]!;
            WebServiceLogging.LoggerFactory.AddLog4Net(loggingConfig);
            logger.Log(LogLevel.Debug, "Configuration found");
            // Set Port 
            SimpleWebService.Port = (uint) Config["server"]!["port"]!;
            // Set often used config values as properties
            // API Key
            MapApiKey = (string) Config["server"]!["mapquest-api-key"]!;
            // Directory where to store route information images
            var routePath = (string) Config["server"]!["routes-path"]!;
            if (!Path.IsPathRooted(routePath))
                routePath = $"{runningPath}{routePath}";
            if (!Directory.Exists(routePath))
                Directory.CreateDirectory(routePath);
            RoutePath = routePath;
            // Directory where to store exports
            var exportPath = (string) Config["server"]!["exports-path"]!;
            if (!Path.IsPathRooted(routePath))
                exportPath = $"{runningPath}{routePath}";
            if (!Directory.Exists(exportPath))
                Directory.CreateDirectory(exportPath);
            ExportPath = exportPath;
            // Postgres config
            var user = (string) Config["server"]!["db"]!["user"]!;
            var password = (string) Config["server"]!["db"]!["password"]!;
            var ip = (string) Config["server"]!["db"]!["ip"]!;
            var port = (string) Config["server"]!["db"]!["port"]!;
            PostgresConnString = $"Server={ip};Port={port};User Id={user};Password={password};";
            logger.Log(LogLevel.Debug, "Configuration read and initialized");
        }
        
        /// <summary>
        /// Use this constructor only for debug purposes.
        /// </summary>
        /// <param name="routePath"></param>
        /// <param name="exportPath"></param>
        /// <param name="connString"></param>
        public Configuration(string routePath, string exportPath, string connString)
        {
            RoutePath = routePath;
            ExportPath = exportPath;
            PostgresConnString = connString;
            Config = new JObject();
            MapApiKey = string.Empty;
        }
    }
}