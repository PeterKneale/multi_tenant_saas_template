namespace Core.Infrastructure.Emails;

public class EmailSenderOptions
{
    public const string SectionName = "EmailSender";
    public bool Enabled { get; init; }
    public string Token { get; init; } = string.Empty;
}