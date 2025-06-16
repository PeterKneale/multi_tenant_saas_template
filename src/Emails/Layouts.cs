using PostmarkDotNet;
using PostmarkDotNet.Model;

namespace Emails;

public static class Layouts
{
    public static async Task Export(PostmarkTemplate template, string env)
    {
        var alias = template.Alias;
        var path = $"{env}/layouts/{alias}";
        Console.WriteLine($"Exporting layout {alias} to ({path})");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        await File.WriteAllTextAsync($"{path}/body.html", template.HtmlBody);
        await File.WriteAllTextAsync($"{path}/body.txt", template.TextBody);
    }

    public static async Task Import(PostmarkClient client, string path)
    {
        var name = Path.GetFileName(path);
        var alias = name;
        Console.WriteLine($"Importing layout {alias} from ({path})");
        var subject = null as string; // Subject is not required for layouts
        var htmlBody = await File.ReadAllTextAsync($"{path}/body.html");
        var textBody = await File.ReadAllTextAsync($"{path}/body.txt");

        await client.CreateTemplateAsync(name, subject, htmlBody, textBody, alias, TemplateType.Layout);
        Console.WriteLine($"Imported layout {alias}");
    }
}