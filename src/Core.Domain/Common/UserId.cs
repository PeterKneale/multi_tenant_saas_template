namespace Core.Domain.Common;

public readonly record struct UserId(Guid Value)
{
    public static readonly UserId Empty = new(Guid.Empty);

    public static UserId Create(Guid value)
    {
        return new UserId(value);
    }
}