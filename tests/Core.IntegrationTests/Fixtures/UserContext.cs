namespace Core.IntegrationTests.Fixtures;

public record UserContext(Guid OrganisationId, Guid UserId, string Email, string Password);