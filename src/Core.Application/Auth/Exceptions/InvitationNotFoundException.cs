namespace Core.Application.Auth.Exceptions;

public class InvitationNotFoundException(InvitationId id) :
    BusinessRuleBrokenException($"An invitation with id {id} not found");