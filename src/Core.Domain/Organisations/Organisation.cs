using Core.Domain.Organisations.Contracts;
using Core.Domain.Organisations.Rules;

namespace Core.Domain.Organisations;

public class Organisation : BaseEntity
{
    private Organisation()
    {
    }

    private Organisation(OrganisationId id, OrganisationName name)
    {
        Id = id;
        Name = name;
        CreatedAt = SystemTime.UtcNow();
    }

    public DateTimeOffset CreatedAt { get; private init; }

    public virtual OrganisationId Id { get; }

    public OrganisationName Name { get; private set; }

    public static Organisation Create(OrganisationId id, OrganisationName name, IOrganisationNameCheck check)
    {
        CheckRule(new NameMustNotBeUsed(name, check));
        return new Organisation(id, name);
    }

    public void ChangeName(OrganisationName name, IOrganisationNameCheck check)
    {
        CheckRule(new NameMustNotBeUsedByOthers(Id, name, check));
        Name = name;
    }
}