using Core.Application.Auth.Commands;
using Core.Application.Users.Queries;

namespace Web.Pages.Auth;

public class Reset(IMediator mediator, ILogger<Reset> logs) : PageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string Token { get; set; }

    [Display(Name = "Password")]
    [Required]
    [BindProperty]
    [MinLength(MinimumPasswordLength, ErrorMessage = MinimumPasswordErrorMessage)]
    [MaxLength(MaximumPasswordLength, ErrorMessage = MaximumPasswordErrorMessage)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var forgotten = await mediator.Send(new HasUserForgottenPassword.Query(UserId));
            if (forgotten) return Page();

            TempData.AddAlert(Alert.Success("Your password has been reset, please login to continue"));
            return RedirectToPage("/Auth/Login");
        }
        catch (BusinessRuleBrokenException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await mediator.Send(new ResetPassword.Command(UserId, Token, Password));
            TempData.AddAlert(Alert.Success("Your password has been reset, please login to continue"));
            return Redirect(nameof(Login));
        }
        catch (BusinessRuleBrokenException e)
        {
            logs.LogWarning("Resetting your password was not successful: {Message}", e.Message);
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }
}