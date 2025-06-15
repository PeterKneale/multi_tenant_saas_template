namespace Web.IntegrationTests.Fixtures;

public class FakeCurrentContext : ICurrentContext
{
    private OrganisationId? _organisationId;
    private UserId? _userId;

    public UserId UserId
    {
        get => _userId ?? throw new Exception("No user context set");
        set => _userId = value;
    }

    public OrganisationId OrganisationId
    {
        get => _organisationId ?? throw new Exception("No organisation context set");
        set => _organisationId = value;
    }

    public void Override(OrganisationId organisationId, UserId userId)
    {
        _organisationId = organisationId;
        _userId = userId;
    }
}