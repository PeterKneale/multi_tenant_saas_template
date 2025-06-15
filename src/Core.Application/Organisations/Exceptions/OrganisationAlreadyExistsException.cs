namespace Core.Application.Organisations.Exceptions;

public class OrganisationAlreadyExistsException(OrganisationId id) :
    BusinessRuleBrokenException($"An organisation with id {id} already exists");