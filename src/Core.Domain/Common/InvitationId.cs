namespace Core.Domain.Common;

public readonly record struct InvitationId(Guid Value)
{
    public static readonly InvitationId Empty = new(Guid.Empty);

    public static InvitationId Create(Guid value)
    {
        return new InvitationId(value);
    }
}