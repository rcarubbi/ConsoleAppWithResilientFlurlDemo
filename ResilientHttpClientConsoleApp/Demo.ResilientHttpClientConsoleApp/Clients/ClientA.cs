using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.ResilientHttpClientConsoleApp.Clients;

public class ClientA([FromKeyedServices(nameof(ClientA))] IFlurlClient flurlClient) : IClientA
{
    public async Task<IEnumerable<WeatherForecast>> GetWeaherInfo()
    {
        var response = await flurlClient.Request("WeatherForecast").GetJsonAsync<IEnumerable<WeatherForecast>>();
        return response;
    }
}
