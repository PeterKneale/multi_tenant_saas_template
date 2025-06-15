namespace Core.Application.Contracts;

public interface ICurrentContext
{
    UserId UserId { get; }

    OrganisationId OrganisationId { get; }
}