using Carubbi.ConsoleApp;
using Carubbi.ConsoleApp.Extensions;
using Carubbi.ResilientFlurl.Extensions;
using Demo.ResilientHttpClientConsoleApp.Clients;
using Demo.ResilientHttpClientConsoleApp.Clients.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Demo.ResilientHttpClientConsoleApp;

public static class ResilientHttpClientConsoleApplicationFactory
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

        builder.Services.AddJwtTokenAuth<ClientA>(configuration);
        builder.Services.AddResilientHttpClient<ClientA>(configuration, httpClientBuilder => httpClientBuilder.AddHttpMessageHandler<JwtDelegatingHandler<ClientA>>());
        builder.Services.AddResilientHttpClient<ClientB>(configuration);

        builder.Services.AddSingleton<IClientA, ClientA>();
        builder.Services.AddSingleton<IClientB, ClientB>();
        
        builder.Services.AddSingleton<Arguments>();
        builder.Services.AddConsoleApp<DemoResilientClientsConsoleApp>();

        var app = builder.Build();
       
        return app;
    }
}
