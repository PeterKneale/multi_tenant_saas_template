namespace Core.Domain.Common;

public class EmailAddress
{
    private EmailAddress(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static EmailAddress Create(string value)
    {
        return new EmailAddress(value.ToLowerInvariant());
    }

    public static implicit operator string(EmailAddress email)
    {
        return email.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}