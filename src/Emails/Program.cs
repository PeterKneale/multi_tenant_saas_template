namespace Emails;

public static class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Please provide both the environment (dev, test, prod) and the action (Import, Export) as arguments.");
            return;
        }

        var environmentName = args[0];
        var action = args[1];

        var env = new Environment(environmentName);

        if (action.Equals("Export", StringComparison.OrdinalIgnoreCase))
        {
            await env.Export();
        }
        else if (action.Equals("Import", StringComparison.OrdinalIgnoreCase))
        {
            await env.Import();
        }
        else
        {
            Console.WriteLine("Invalid action specified. Please use 'Import' or 'Export'.");
        }
    }
}