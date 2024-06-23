using Carubbi.ConsoleApp;
using Serilog;

namespace Demo.ConsoleApp.Tests;

public class ConsoleApplicationFactoryShould
{
    [Fact]
    public void CreateApp()
    {
        var app = ConsoleApplicationFactory.CreateApp(["--v 1.0.0"]);

        // assert
        app.Should().NotBeNull();
        app.Should().BeOfType<ConsoleApplication>();
        Log.Logger.Should().NotBeNull();
    }
}
