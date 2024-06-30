using Demo.SimpleConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleAppDemo.Tests;

public class DemoSimpleConsoleAppShould : TestBase<DemoSimpleConsoleApp>
{
    private readonly Mock<IDateTimeWrapper> _dateTimeWrapperMock = new();
    protected override DemoSimpleConsoleApp ArrangeSut()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            { nameof(Arguments.Version), "1.0.0"}
        }!).Build();

        return new DemoSimpleConsoleApp(new Arguments(configuration), LoggerFactory, _dateTimeWrapperMock.Object);
    }

    [Fact]
    public async Task LogVersionAndTimes_WhenStarted()
    {
        // arrange
        var now = new DateTime(2021, 1, 1, 2, 1, 1);
        _dateTimeWrapperMock.Setup(x => x.Now).Returns(now);
        var utcNow = new DateTime(2021, 1, 1, 1, 1, 1);
        _dateTimeWrapperMock.Setup(x => x.UtcNow).Returns(utcNow);


        // act

        await Sut.RunAsync();

        // assert
        _dateTimeWrapperMock.Verify(x => x.Now, Times.Once);
        _dateTimeWrapperMock.Verify(x => x.UtcNow, Times.Once);


        LoggerMock.Verify(
              x => x.Log(
                  LogLevel.Information,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? string.Empty).Contains("Running DemoConsoleApp")),
                  null,
                  (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
              Times.Once);


        LoggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? string.Empty).Contains("Version: 1.0.0")),
                null,
               (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);

        LoggerMock.Verify(
          x => x.Log(
              LogLevel.Information,
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? string.Empty).Contains("DateTime.Now: 01/01/2021 02:01:01")),
              null,
             (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
          Times.Once);

        LoggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? string.Empty).Contains("DateTime.UtcNow: 01/01/2021 01:01:01")),
                null,
               (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);

        LoggerMock.Verify(
         x => x.Log(
             LogLevel.Information,
             It.IsAny<EventId>(),
             It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? string.Empty).Contains("")),
             null,
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
         Times.AtLeastOnce);

        LoggerMock.Verify(
          x => x.Log(
              LogLevel.Information,
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? string.Empty).Contains("DemoConsoleApp completed successfully")),
              null,
             (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
          Times.Once);


        VerifyNoOtherDependenciesCalls();
    }



    private void VerifyNoOtherDependenciesCalls()
    {
        _dateTimeWrapperMock.VerifyNoOtherCalls();
        LoggerMock.VerifyNoOtherCalls();
    }
}