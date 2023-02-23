using Microsoft.Extensions.Logging;

namespace FileProcessor.Logic.Utilities;

internal static class LoggingManager
{
    static LoggingManager()
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        Logger = loggerFactory.CreateLogger<ProcessingService>();
    }

    public static ILogger Logger { get; }
}