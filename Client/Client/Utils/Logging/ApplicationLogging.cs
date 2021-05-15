using Microsoft.Extensions.Logging;

namespace Client.Utils.Logging
{
    /// <summary>
    /// Provides an easy way to setup logging in the application.
    /// Based on: https://docs.microsoft.com/en-us/archive/msdn-magazine/2016/april/essential-net-logging-with-net-core
    /// </summary>
    public static class ApplicationLogging
    {
        public static ILoggerFactory LoggerFactory {get;} = new LoggerFactory();
        
        /// <summary>
        /// Creates a new logger of given type.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the new logger.
        /// </typeparam>
        /// <returns>
        /// Logger of the given type.
        /// </returns>
        public static ILogger CreateLogger<T>() =>
            LoggerFactory.CreateLogger<T>();
    }
}