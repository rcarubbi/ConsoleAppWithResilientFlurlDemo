using Carubbi.ConsoleApp.Enums;
using Carubbi.ConsoleApp.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.MultipleAppsConsoleApp;

public class ConsoleAppB(ILoggerFactory loggerFactory) : IConsoleApp
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ConsoleAppB>();

    public async Task<ExitCodes> RunAsync()
    {
        _logger.LogInformation("Running Console App B");

        foreach(var i in Enumerable.Range(1, 20))
        {
            _logger.LogInformation("Console App B: {number}", i);
            await Task.Delay(250);
        }
      
        _logger.LogInformation("Console App B completed successfully");
     
        return ExitCodes.Ok;
    }
}
