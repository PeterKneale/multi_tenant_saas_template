using Core.Application.Contracts;

namespace Core.Infrastructure.Services;

public class CommandQueue(ILogger<CommandQueue> logs) : ICommandQueue
{
    public async Task QueueHighPriorityCommand(IRequest command, CancellationToken cancellationToken)
    {
        await DoQueueCommand(command, cancellationToken);
    }

    public async Task QueueMediumPriorityCommand(IRequest command, CancellationToken cancellationToken)
    {
        await DoQueueCommand(command, cancellationToken);
    }

    public async Task QueueLowPriorityCommand(IRequest command, CancellationToken cancellationToken)
    {
        await DoQueueCommand(command, cancellationToken);
    }

    private async Task DoQueueCommand(IRequest command, CancellationToken cancellationToken)
    {
        var name = command.GetType().FullName.Split(".").Last();
        logs.LogInformation($"⏲️ Queuing command: {name}");
    }
}