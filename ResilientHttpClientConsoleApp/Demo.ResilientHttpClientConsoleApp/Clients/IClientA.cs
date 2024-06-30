namespace Demo.ResilientHttpClientConsoleApp.Clients;

public interface IClientA
{
    Task<IEnumerable<WeatherForecast>> GetWeaherInfo();
}
