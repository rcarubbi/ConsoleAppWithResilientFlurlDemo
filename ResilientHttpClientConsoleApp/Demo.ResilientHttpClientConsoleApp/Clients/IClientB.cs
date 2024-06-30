namespace Demo.ResilientHttpClientConsoleApp.Clients;

public interface IClientB
{
    Task<IEnumerable<WeatherForecast>> GetWeaherInfo();
}
