namespace Demo.ConsoleApp;

public interface IDateTimeWrapper
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}
