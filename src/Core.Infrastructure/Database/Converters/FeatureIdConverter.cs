using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Database.Converters;

public class FeatureIdConverter()
    : ValueConverter<FeatureId, Guid>(id => id.Value, guid => new FeatureId(guid));