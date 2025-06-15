using Core.Application.Users.Commands;

namespace Web.Pages.Auth;

public class Verify(IMediator mediator, ILogger<Login> logs) : PageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string Verification { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await mediator.Send(new VerifyEmailAddress.Command(UserId, Verification));
            TempData.AddAlert(Alert.Success("Your account has been verified, please login to continue"));
            return Redirect(nameof(Login));
        }
        catch (BusinessRuleBrokenException e)
        {
            logs.LogWarning("Verification was not successful: {Message}", e.Message);
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }
}