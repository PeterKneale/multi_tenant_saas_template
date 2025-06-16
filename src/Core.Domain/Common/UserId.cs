namespace Core.Domain.Common;

public readonly record struct UserId(Guid Value)
{
    public static readonly UserId Empty = new(Guid.Empty);

    public static UserId Create(Guid value)
    {
        return new UserId(value);
    }

    public static UserId Create() => Create(Guid.NewGuid());
    
    public static implicit operator Guid(UserId userId) => userId.Value;
    public static implicit operator UserId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}