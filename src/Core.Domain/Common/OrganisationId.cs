namespace Core.Domain.Common;

public readonly record struct OrganisationId(Guid Value)
{
    public static readonly OrganisationId Empty = new(Guid.Empty);

    public static OrganisationId Create(Guid value)
    {
        return new OrganisationId(value);
    }

    public static OrganisationId Create() => Create(Guid.NewGuid());
}