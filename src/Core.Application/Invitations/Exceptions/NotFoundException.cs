namespace Core.Application.Invitations.Exceptions;

public class NotFoundException : BusinessRuleBrokenException
{
    public NotFoundException(string name, string value) : base($"{name} {value} not found")
    {
    }

    public NotFoundException(string name, Guid value) : base($"{name} {value} not found")
    {
    }
}