
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.ConsoleApp.Clients;

public class ClientB([FromKeyedServices(nameof(ClientB))] IFlurlClient flurlClient) : IClientB
{
    public async Task<IEnumerable<WeatherForecast>> GetWeaherInfo()
    {
        var response = await flurlClient.Request("WeatherForecast").GetJsonAsync<IEnumerable<WeatherForecast>>();
        return response;
    }
}
