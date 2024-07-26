using Microsoft.Extensions.Logging;

public static class Logger
{
    private static ILogger _logger;

    public static void Configure(ILogger logger)
    {
        _logger = logger;
    }

    public static void LogInfo(string message)
    {
        _logger?.LogInformation(message);
    }

    public static void LogError(string message, Exception ex = null)
    {
        _logger?.LogError(ex, message);
    }
}
