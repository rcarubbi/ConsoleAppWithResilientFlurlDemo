namespace Demo.ConsoleApp.Clients;

public interface IClientA
{
    Task<IEnumerable<WeatherForecast>> GetWeaherInfo();
}
