using Newtonsoft.Json;
using PostmarkDotNet;

namespace Core.Infrastructure.Emails;

public class EmailSender(EmailSenderOptions options, ILogger<EmailSender> log)
{
    public async Task SendMessage(TemplatedPostmarkMessage message)
    {
        var body = JsonConvert.SerializeObject(message.TemplateModel);

        log.LogInformation($"✉️ Sending {message.TemplateAlias} email to {message.To}\n{body}");

        if (!options.Enabled)
        {
            log.LogInformation("✋ Emails sending is NOT enabled");
            return;
        }

        try
        {
            var client = new PostmarkClient(options.Token);
            var result = await client.SendMessageAsync(message);

            if (result.Status != PostmarkStatus.Success)
            {
                var error = $"💥Postmark reported failure: {result.Status} {result.ErrorCode} {result.MessageID} {result.Message}";
                log.LogError(error);
                throw new Exception(error);
            }
        }
        catch (Exception exception)
        {
            log.LogError(exception, "💥Error sending email {Message}", exception.Message);
            throw;
        }
    }
}