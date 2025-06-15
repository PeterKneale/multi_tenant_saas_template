using Core.Application.Contracts;
using Core.Domain.Common;

namespace Web.Code;

public class CurrentContext(IHttpContextAccessor accessor) : ICurrentContext
{
    private OrganisationId? _organisationId;
    private UserId? _userId;

    public UserId UserId => _userId ?? accessor.HttpContext.GetCurrentUser().Id;

    public OrganisationId OrganisationId => _organisationId ?? accessor.HttpContext.GetCurrentOrganisation().Id;

    public void Override(OrganisationId organisationId, UserId userId)
    {
        _organisationId = organisationId;
        _userId = userId;
    }
}