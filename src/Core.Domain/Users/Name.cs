namespace Core.Domain.Users;

public record Name(string FirstName, string LastName)
{
    public string FirstName { get; } =
        FirstName ?? throw new ArgumentNullException(nameof(FirstName), "first name is null");

    public string LastName { get; } =
        LastName ?? throw new ArgumentNullException(nameof(LastName), "last name is null");

    public string FullName => $"{FirstName} {LastName}";
    public string Initials => $"{FirstName[0]}{LastName[0]}";
}