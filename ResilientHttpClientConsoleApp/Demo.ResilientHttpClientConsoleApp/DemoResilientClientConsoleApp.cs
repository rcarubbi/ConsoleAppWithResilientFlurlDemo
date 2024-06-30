using Carubbi.ConsoleApp.Enums;
using Carubbi.ConsoleApp.Interfaces;
using Demo.ResilientHttpClientConsoleApp.Clients;
using Microsoft.Extensions.Logging;

namespace Demo.ResilientHttpClientConsoleApp;

public class DemoResilientClientsConsoleApp(Arguments arguments, ILoggerFactory loggerFactory, IClientA clientA, IClientB clientB) : IConsoleApp
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<DemoResilientClientsConsoleApp>();

    public async Task<ExitCodes> RunAsync()
    {
        _logger.LogInformation("Running DemoConsoleApp");

        _logger.LogInformation("Version: {Version}", arguments.Version);

        await StartClientRequests(clientA.GetWeaherInfo);
        
        await StartClientRequests(clientA.GetWeaherInfo);

        await Task.Delay(10000);
        await StartClientRequests(clientA.GetWeaherInfo);
        //await StartClientRequests(clientB.GetWeaherInfo);

        _logger.LogInformation("DemoConsoleApp completed successfully");
        return ExitCodes.Ok;
    }

    private async Task StartClientRequests(Func<Task<IEnumerable<WeatherForecast>>> requestHandler)
    {
        Task<IEnumerable<WeatherForecast>>[] tasks = Enumerable.Range(0, 30).Select(_ => requestHandler()).ToArray();
        var results = await Task.WhenAll(tasks);
        var output = results.Where(i => i != null).SelectMany(_ => _).ToList();

        _logger.LogInformation("client output: {@clientAResponse}", output);
        _logger.LogInformation("client output count: {count}", output.Count);
    }
}
