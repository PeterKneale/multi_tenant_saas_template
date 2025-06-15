using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Database.Converters;

public class EmailAddressConverter : ValueConverter<EmailAddress, string>
{
    public EmailAddressConverter() : base(obj => obj.Value,
        str => EmailAddress.Create(str))
    {
    }
}