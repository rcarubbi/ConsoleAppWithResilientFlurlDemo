namespace Demo.SimpleConsoleApp;

public interface IDateTimeWrapper
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}
