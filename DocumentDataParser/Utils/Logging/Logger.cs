using Microsoft.Extensions.Logging;

namespace DocumentDataParser.Utils.Logging
{

}
public class Logger
{
    private static ILogger<Logger> _logger;
    public static void InitializeLogger(ILogger<Logger> logger)
    {
        _logger = logger;
    }

    public static void LogInfo(string message)
    {
        if (_logger != null)
        {
            _logger.LogInformation(message);
        }
    }

    public static void LogError(string message, Exception ex)
    {
        if (_logger != null)
        {
            _logger.LogError(ex, message);
        }
    }
}
