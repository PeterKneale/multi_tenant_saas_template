using System.Diagnostics;

namespace Core.Domain.Features;

[DebuggerDisplay("{Title} ({Description})")]
public record FeatureName(string Title, string? Description = null)
{
    public string Title { get; } = Title ?? throw new ArgumentNullException(nameof(Title), "title is null");
    public string? Description { get; } = Description;
}