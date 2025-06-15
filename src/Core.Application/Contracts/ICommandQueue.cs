namespace Core.Application.Contracts;

public interface ICommandQueue
{
    Task QueueHighPriorityCommand(IRequest command, CancellationToken cancellationToken);
    Task QueueMediumPriorityCommand(IRequest command, CancellationToken cancellationToken);
    Task QueueLowPriorityCommand(IRequest command, CancellationToken cancellationToken);
}