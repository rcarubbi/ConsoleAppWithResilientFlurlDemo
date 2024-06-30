using Carubbi.ConsoleApp.Enums;
using Carubbi.ConsoleApp.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.MultipleAppsConsoleApp;

public class ConsoleAppA(ILoggerFactory loggerFactory) : IConsoleApp
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ConsoleAppA>();

    public async Task<ExitCodes> RunAsync()
    {
        _logger.LogInformation("Running Console App A");

        foreach(var i in Enumerable.Range(1, 10))
        {
            _logger.LogInformation("Console App A: {number}", i);
            await Task.Delay(500);
        }
      
        _logger.LogInformation("Console App A completed successfully");
     
        return ExitCodes.Ok;
    }
}
