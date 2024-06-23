using Carubbi.ConsoleApp.Enums;
using Carubbi.ConsoleApp.Interfaces;
using Demo.ConsoleApp.Clients;
using Microsoft.Extensions.Logging;

namespace Demo.ConsoleApp;

public class DemoConsoleApp(Arguments arguments, ILoggerFactory loggerFactory, IDateTimeWrapper dateTimeWrapper, IClientA clientA, IClientB clientB) : IConsoleApp
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<DemoConsoleApp>();

    public async Task<ExitCodes> RunAsync()
    {
        _logger.LogInformation("Running DemoConsoleApp");
               
        _logger.LogInformation( "Version: {Version}", arguments.Version);

        // log date time now and utc now
        _logger.LogInformation("DateTime.Now: {DateTimeNow}", dateTimeWrapper.Now);
        _logger.LogInformation( "DateTime.UtcNow: {DateTimeUtcNow}", dateTimeWrapper.UtcNow);

        Task<IEnumerable<WeatherForecast>>[] tasks = Enumerable.Range(0, 100).Select(_ => clientA.GetWeaherInfo()).ToArray();
        var results = await Task.WhenAll(tasks);
        _logger.LogInformation("{@clientAResponse}", results.SelectMany(_ => _));

        Task<IEnumerable<WeatherForecast>>[] tasksB = Enumerable.Range(0, 100).Select(_ => clientB.GetWeaherInfo()).ToArray();
        var resultsB = await Task.WhenAll(tasksB);
        _logger.LogInformation("{@clientAResponse}", resultsB.SelectMany(_ => _));

        _logger.LogInformation("DemoConsoleApp completed successfully");

        return ExitCodes.Ok;
    }
}
