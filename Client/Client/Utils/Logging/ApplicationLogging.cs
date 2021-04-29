using Microsoft.Extensions.Logging;

namespace Client.Utils.Logging
{
    public static class ApplicationLogging
    {
        public static ILoggerFactory LoggerFactory {get;} = new LoggerFactory();
        public static ILogger CreateLogger<T>() =>
            LoggerFactory.CreateLogger<T>();
    }
}