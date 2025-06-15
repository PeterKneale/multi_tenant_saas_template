namespace Core.Application.Organisations.Exceptions;

public class OrganisationNotFoundException : BusinessRuleBrokenException
{
    public OrganisationNotFoundException(OrganisationId id) : base($"An organisation with id {id} not found")
    {
    }

    public OrganisationNotFoundException() : base("Organisation not found")
    {
    }
}