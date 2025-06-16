using AwesomeAssertions;
using Core.Domain.Users;
using Core.Infrastructure.Emails;
using Core.Infrastructure.Emails.Models;
using Xunit.Abstractions;
using static Core.Infrastructure.UnitTests.Emails.Builders;

namespace Core.Infrastructure.UnitTests.Emails;

public class VerifyEmailTests(ITestOutputHelper output)
{
    [Fact]
    public void NameIsCorrect() => BuildModel()
        .ToName
        .Should().Be("First Last");
    
    [Fact]
    public void ProductNameIsCorrect() => BuildModel()
        .ProductName
        .Should().Be("Product Name");

    [Fact]
    public void FromEmailIsCorrect() => BuildModel()
        .FromEmail
        .Should().Be("NoReply@localhost.com");
    
    [Fact]
    public void SupportEmailIsCorrect() => BuildModel()
        .SupportEmail
        .Should().Be("help@localhost.com");
    
    [Fact]
    public void HelpUrlIsCorrect() => BuildModel()
        .HelpUrl
        .Should().MatchEquivalentOf("http://localhost.com/help");

    [Fact]
    public void ActionUrlIsCorrect()
    {
        var user = BuildUser();
        BuildModel(user: user)
            .ActionUrl
            .Should().MatchEquivalentOf($"http://localhost.com/Auth/Verify" +
                                        $"?UserId={user.Id.Value}" +
                                        $"&verification={user.VerifiedToken}");
    }

    [Fact]
    public void ToAddressIsLowered() => BuildModel(user: BuildUser(email: "BOB@EXAMPLE.COM"))
        .ToEmail
        .Should().BeLowerCased();
    
    [Fact]
    public void SupportEmailIsLowered() =>
        BuildModel(options: BuildEmailOptions(supportEmail: "HELP@localhost.COM"))
            .SupportEmail
            .Should().BeLowerCased();

    private VerifyEmailAddressEmail BuildModel(EmailBuilderOptions? options = null, User? user = null)
    {
        var builder = new EmailBuilder(options ?? BuildEmailOptions());
        var model = builder.BuildVerifyEmailAddress(user ?? BuildUser());
        output.WriteLine(model.ToString());
        return model;
    }
}