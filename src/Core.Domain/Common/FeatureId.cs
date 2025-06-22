namespace Core.Domain.Common;

public readonly record struct FeatureId(Guid Value)
{
    public static readonly FeatureId Empty = new(Guid.Empty);

    public static FeatureId Create(Guid value)
    {
        return new FeatureId(value);
    }

    public static FeatureId Create() => Create(Guid.NewGuid());
    
    public static implicit operator Guid(FeatureId FeatureId) => FeatureId.Value;
    public static implicit operator FeatureId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}