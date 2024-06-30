using Carubbi.ConsoleApp;
using Serilog;

namespace Demo.SimpleConsoleApp.Tests;

public class ConsoleApplicationFactoryShould
{
    [Fact]
    public void CreateApp()
    {
        var app = SimpleConsoleApplicationFactory.CreateApp(["--v 1.0.0"]);

        // assert
        app.Should().NotBeNull();
        app.Should().BeOfType<ConsoleApplication>();
        Log.Logger.Should().NotBeNull();
    }
}
