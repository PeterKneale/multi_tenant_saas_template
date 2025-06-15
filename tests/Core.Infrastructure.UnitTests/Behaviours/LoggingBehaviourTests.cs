using Core.Application;
using Core.Infrastructure.Behaviours;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace Core.Infrastructure.UnitTests.Behaviours;

public class LoggingBehaviourTests
{
    [Fact]
    public async Task Sensitive_data_should_not_be_logged()
    {
        // Arrange
        var logger = new Mock<ILogger<SensitiveCommand>>();
        var behaviour = new LoggingBehaviour<SensitiveCommand, Unit>(logger.Object);
        var command = new SensitiveCommand();
        var next = new RequestHandlerDelegate<Unit>(_ => Task.FromResult(Unit.Value));

        // Act
        await behaviour.Handle(command, next, CancellationToken.None);

        // Assert
        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("*** Sensitive Data ***")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task Normal_data_can_be_logged()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<NormalCommand>>();
        var behaviour = new LoggingBehaviour<NormalCommand, Unit>(loggerMock.Object);
        var command = new NormalCommand { Data = "Test" };
        var next = new RequestHandlerDelegate<Unit>(_ => Task.FromResult(Unit.Value));

        // Act
        await behaviour.Handle(command, next, CancellationToken.None);

        // Assert
        loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(JsonConvert.SerializeObject(command))),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [SensitiveData]
    public class SensitiveCommand : IRequest<Unit>;

    public class NormalCommand : IRequest<Unit>
    {
        public string Data { get; set; }
    }
}