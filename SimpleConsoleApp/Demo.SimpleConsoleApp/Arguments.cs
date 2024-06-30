using Microsoft.Extensions.Configuration;

namespace Demo.SimpleConsoleApp;

public class Arguments(IConfiguration configuration)
{
    public static IDictionary<string, string>? SwitchMap { get; internal set; } = new Dictionary<string, string>()
    {
        {"--v", nameof(Version)}
    };

    public string Version { get; init; } = configuration![nameof(Version)]!;
}
