using System.Text.RegularExpressions;

namespace Web.AcceptanceTests.Pages.Extensions;

public static class LocatorAssertionsExtensions
{
    public static async Task<bool> IsFieldRequiredAsync(this ILocator field) => 
        await field.GetAttributeAsync("required") is not null;
    
    public static async Task LabelMarkedRequiredAsync(this ILocatorAssertions field) => 
        await field.ToContainClassAsync("required");

    public static Task ToBe(this ILocatorAssertions assertions, bool valid) =>
        valid ? assertions.ToBeValid() : assertions.ToBeInvalid();

    private static Task ToBeValid(this ILocatorAssertions assertions) =>
        assertions.ToHaveClassAsync(new Regex("is-valid"));

    private static Task ToBeInvalid(this ILocatorAssertions assertions) =>
        assertions.ToHaveClassAsync(new Regex("is-invalid"));

    public static bool ToBoolean(this string validity) =>
        validity.ToLowerInvariant() switch
        {
            "valid" => true,
            "true" => true,
            "invalid" => false,
            "false" => false,
            _ => throw new NotSupportedException($"Validity {validity} is not supported")
        };
}