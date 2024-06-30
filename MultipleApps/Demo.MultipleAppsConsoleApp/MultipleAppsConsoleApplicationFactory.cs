using Carubbi.ConsoleApp;
using Carubbi.ConsoleApp.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Demo.MultipleAppsConsoleApp;

public static class MultipleAppsConsoleApplicationFactory
{
    public static ConsoleApplication CreateApp(string[] arguments)
    {
        var builder = ConsoleApplication.CreateBuilder(arguments);
        var serviceProvider = builder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(configuration)
          .CreateLogger();
        builder.Services.AddLogging(services => services.AddSerilog());

     
        builder.Services.AddConsoleApp<ConsoleAppA>();
        builder.Services.AddConsoleApp<ConsoleAppB>();

        var app = builder.Build();
       
        return app;
    }
}
