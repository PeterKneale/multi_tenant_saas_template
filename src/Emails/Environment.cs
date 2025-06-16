using PostmarkDotNet;
using PostmarkDotNet.Model;

namespace Emails;

public class Environment(string name)
{
    public string Name { get; init; } = name;

    public async Task Export()
    {
        Console.WriteLine($"Exporting {Name}");
        var client = new PostmarkClient(GetPostmarkApiToken());
        var templates = await client.GetTemplatesAsync();
        foreach (var template in templates.Templates)
        {
            if (template.TemplateType == TemplateType.Layout)
            {
                await Layouts.Export(await client.GetTemplateAsync(template.Alias), Name);
            }
            else
            {
                await Templates.Export(await client.GetTemplateAsync(template.Alias), Name);
            }
        }
    }

    public async Task Import()
    {
        Console.WriteLine($"Importing to {Name}");
        var client = new PostmarkClient(GetPostmarkApiToken());
        foreach (var directory in Directory.GetDirectories(Path.Combine(Name, "layouts")))
        {
            await Layouts.Import(client, directory);
        }

        foreach (var directory in Directory.GetDirectories(Path.Combine(Name, "templates")))
        {
            await Templates.Import(client, directory);
        }
    }

    private static string GetPostmarkApiToken()
    {
        return System.Environment.GetEnvironmentVariable("POSTMARK_API_TOKEN") 
               ?? throw new InvalidOperationException("POSTMARK_API_TOKEN environment variable is not set");
    }
}