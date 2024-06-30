namespace Demo.ResilientHttpClientConsoleApp.Clients.Security;

public interface ITokenProvider<T>
{
    Task<string> GetTokenAsync(CancellationToken cancellationToken);
}
