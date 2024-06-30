using Carubbi.ConsoleApp.Enums;
using Carubbi.ConsoleApp.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.SimpleConsoleApp;

public class DemoSimpleConsoleApp(Arguments arguments, ILoggerFactory loggerFactory, IDateTimeWrapper dateTimeWrapper) : IConsoleApp
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<DemoSimpleConsoleApp>();

    public Task<ExitCodes> RunAsync()
    {
        _logger.LogInformation("Running DemoConsoleApp");

        _logger.LogInformation("Version: {Version}", arguments.Version);

        // log date time now and utc now
        _logger.LogInformation("DateTime.Now: {DateTimeNow}", dateTimeWrapper.Now);
        _logger.LogInformation("DateTime.UtcNow: {DateTimeUtcNow}", dateTimeWrapper.UtcNow);

        _logger.LogInformation("DemoConsoleApp completed successfully");
     
        return Task.FromResult(ExitCodes.Ok);
    }
}
