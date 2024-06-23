using Carubbi.ConsoleApp;
using Carubbi.ConsoleApp.Extensions;
using Carubbi.ResilientFlurl.Extensions;
using Demo.ConsoleApp.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Demo.ConsoleApp
{
    public static class ConsoleApplicationFactory
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
            builder.Services.AddHttpClients(builder.Configuration);
            builder.Services.AddSingleton<IClientA, ClientA>();
            builder.Services.AddSingleton<IClientB, ClientB>();
            builder.Services.AddSingleton<IDateTimeWrapper, SystemDateTimeWrapper>();
            builder.Services.AddSingleton<Arguments>();
            builder.Services.AddConsoleApp<DemoConsoleApp>();

            var app = builder.Build();
           
            return app;
        }
    }
}
