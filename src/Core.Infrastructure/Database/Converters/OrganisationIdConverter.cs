using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Database.Converters;

public class OrganisationIdConverter()
    : ValueConverter<OrganisationId, Guid>(id => id.Value, guid => new OrganisationId(guid));