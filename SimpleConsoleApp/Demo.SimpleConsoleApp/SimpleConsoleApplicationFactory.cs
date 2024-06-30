using Carubbi.ConsoleApp;
using Carubbi.ConsoleApp.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Demo.SimpleConsoleApp;

public static class SimpleConsoleApplicationFactory
{
    public static ConsoleApplication CreateApp(string[] arguments)
    {
        var builder = ConsoleApplication.CreateBuilder(arguments, Arguments.SwitchMap);
        var serviceProvider = builder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(configuration)
          .CreateLogger();
        builder.Services.AddLogging(services => services.AddSerilog());

        builder.Services.AddSingleton<IDateTimeWrapper, SystemDateTimeWrapper>();
        builder.Services.AddSingleton<Arguments>();
        builder.Services.AddConsoleApp<DemoSimpleConsoleApp>();

        var app = builder.Build();
       
        return app;
    }
}
