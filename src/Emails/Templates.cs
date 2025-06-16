using PostmarkDotNet;
using PostmarkDotNet.Model;

namespace Emails;

public static class Templates
{
    public static async Task Export(PostmarkTemplate template, string env)
    {
        var alias = template.Alias;
        var path = $"{env}/templates/{alias}";
        Console.WriteLine($"Exporting template {alias} to ({path})");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        await File.WriteAllTextAsync($"{path}/body.html", template.HtmlBody);
        await File.WriteAllTextAsync($"{path}/body.txt", template.TextBody);
        await File.WriteAllTextAsync($"{path}/subject.txt", template.Subject);
        await File.WriteAllTextAsync($"{path}/layout.txt", template.LayoutTemplate);
    }

    public static async Task Import(PostmarkClient client, string path)
    {
        var name = Path.GetFileName(path);
        var alias = name;
        Console.WriteLine($"Importing template {alias} from ({path})");
        var htmlBody = await File.ReadAllTextAsync($"{path}/body.html");
        var textBody = await File.ReadAllTextAsync($"{path}/body.txt");
        var subject = await File.ReadAllTextAsync($"{path}/subject.txt");
        var layout = await File.ReadAllTextAsync($"{path}/layout.txt");

        await client.CreateTemplateAsync(name, subject, htmlBody, textBody, alias, TemplateType.Standard, layout);
    }
}