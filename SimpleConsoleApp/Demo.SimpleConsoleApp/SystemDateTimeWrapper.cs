namespace Demo.SimpleConsoleApp;

public class SystemDateTimeWrapper : IDateTimeWrapper
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;
}
