namespace Demo.ConsoleApp.Clients;

public interface IClientB
{
    Task<IEnumerable<WeatherForecast>> GetWeaherInfo();
}
