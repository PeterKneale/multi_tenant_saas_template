namespace Core.Domain.Common;

public readonly record struct ProjectId(Guid Value)
{
    public static readonly ProjectId Empty = new(Guid.Empty);

    public static ProjectId Create(Guid value)
    {
        return new ProjectId(value);
    }

    public static ProjectId Create() => Create(Guid.NewGuid());
    
    public static implicit operator Guid(ProjectId ProjectId) => ProjectId.Value;
    public static implicit operator ProjectId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}