using Carubbi.ConsoleApp.Enums;
using Serilog;
using static Demo.SimpleConsoleApp.SimpleConsoleApplicationFactory;
using static Carubbi.ConsoleApp.Extensions.EnvironmentExtensions;

try
{
    var app = CreateApp(args);
    var result = await app.RunAsync();
    Console.ReadKey();
    Exit(result);
  
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