namespace Core.Domain.Users;

public record UserRole
{
    public const string AdminRoleName = "Admin";

    public const string OwnerRoleName = "Owner";

    public const string MemberRoleName = "Member";

    private UserRole(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static UserRole Admin => Create(AdminRoleName);

    public static UserRole Owner => Create(OwnerRoleName);

    public static UserRole Member => Create(MemberRoleName);

    public static UserRole Create(string value)
    {
        return new UserRole(value);
    }

    public static implicit operator string(UserRole role)
    {
        return role.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}