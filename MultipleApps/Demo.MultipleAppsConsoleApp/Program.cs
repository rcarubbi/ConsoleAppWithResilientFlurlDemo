using Carubbi.ConsoleApp.Enums;
using Demo.MultipleAppsConsoleApp;
using Serilog;
using static Carubbi.ConsoleApp.Extensions.EnvironmentExtensions;
using static Demo.MultipleAppsConsoleApp.MultipleAppsConsoleApplicationFactory;

try
{
    var app = CreateApp(args);
    Log.Information("Running in parallel:");
    var result1 = await app.RunInParallelAsync();

    Log.Information("Running sequencially:");
    var result2 = await app.RunSequenciallyAsync();


    Log.Information("cherry-picking app to run:");
    var result3 = await app.RunAsync<ConsoleAppB>();
    var result4 = await app.RunAsync<ConsoleAppA>();

    Console.ReadKey();
    Exit(result1);

}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred");
    Console.ReadKey();
    Exit(ExitCodes.Error);

}
finally
{
    await Task.Delay(TimeSpan.FromSeconds(10)); // wait some time to ensure logs are flushed as application insights takes some time to send logs
    await Log.CloseAndFlushAsync();

}