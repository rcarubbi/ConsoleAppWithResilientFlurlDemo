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

        _logger.LogInformation("Version: {Version}", arguments.Version);

        // log date time now and utc now
        _logger.LogInformation("DateTime.Now: {DateTimeNow}", dateTimeWrapper.Now);
        _logger.LogInformation("DateTime.UtcNow: {DateTimeUtcNow}", dateTimeWrapper.UtcNow);

        await StartClientRequests(clientA.GetWeaherInfo);
        await StartClientRequests(clientB.GetWeaherInfo);

        _logger.LogInformation("DemoConsoleApp completed successfully");

        return ExitCodes.Ok;
    }

    private async Task StartClientRequests(Func<Task<IEnumerable<WeatherForecast>>> requestHandler)
    {
        Task<IEnumerable<WeatherForecast>>[] tasks = Enumerable.Range(0, 100).Select(_ => requestHandler()).ToArray();
        var results = await Task.WhenAll(tasks);
        var output = results.Where(i => i != null).SelectMany(_ => _).ToList();

        _logger.LogInformation("client output: {@clientAResponse}", output);
        _logger.LogInformation("client output count: {count}", output.Count);
    }
}
