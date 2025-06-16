using AwesomeAssertions;
using Core.Application.Contracts;
using Core.Infrastructure.Emails;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.UnitTests.Emails;

public class EmailServiceTest
{
    [Fact]
    public void EmailServiceCanBeResolvedWhenEnabled()
    {
        GetEmailService(new Dictionary<string, string?>
        {
            ["EmailSender:Enabled"] = "true",
            ["EmailSender:Token"] = "test-token",
            ["EmailBuilder:ProductName"] = "Test Product",
            ["EmailBuilder:PublicUri"] = "http://localhost.com",
            ["EmailBuilder:HelpUrl"] = "/help",
            ["EmailBuilder:FromEmail"] = "NoReply@localhost.com",
            ["EmailBuilder:SupportEmail"] = "help@localhost.com"
        }).Should().NotBeNull();
    }

    [Fact]
    public void EmailServiceCanBeResolvedWhenDisabled()
    {
        GetEmailService(new Dictionary<string, string?>
        {
            ["EmailSender:Enabled"] = "false",
            ["EmailBuilder:ProductName"] = "Test Product",
            ["EmailBuilder:PublicUri"] = "http://localhost.com",
            ["EmailBuilder:HelpUrl"] = "/help",
            ["EmailBuilder:FromEmail"] = "NoReply@localhost.com",
            ["EmailBuilder:SupportEmail"] = "help@localhost.com"
        }).Should().NotBeNull();
    }

    private static IEmailService GetEmailService(Dictionary<string, string?> config)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
        var container = new ServiceCollection()
            .AddInfrastructure(configuration)
            .BuildServiceProvider();
        var emailService = container.GetRequiredService<IEmailService>();
        return emailService;
    }
}