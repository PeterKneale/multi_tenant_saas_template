using Web.Code.Authentication;
using static Core.Domain.Users.UserConstants;

namespace Web.Pages.Auth;

public class Login(AdminAuthService adminAuth, UserAuthService userAuth, ILogger<Login> logs) : PageModel
{
    [Display(Name = "Email")]
    [Required]
    [BindProperty]
    [StringLength(MaxEmailLength)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Display(Name = "Password")]
    [Required]
    [BindProperty]
    [MinLength(MinimumPasswordLength, ErrorMessage = MinimumPasswordErrorMessage)]
    [MaxLength(MaximumPasswordLength, ErrorMessage = MaximumPasswordErrorMessage)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    /// <summary>
    ///     This field is a honeypot to prevent bots from logging in
    /// </summary>
    [BindProperty]
    [StringLength(100)]
    public string? Name { get; set; }

    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        if (!ModelState.IsValid) return Page();

        // If the honeypot field is filled in, then the user is a bot
        if (Name != null)
        {
            logs.LogWarning("Bot detected entering : {Value} in honeypot field", Name);
            TempData.AddAlert(Alert.Danger("Bot detected"));
            return Page();
        }

        var adminResult = await adminAuth.Authenticate(Email, Password);
        if (adminResult.IsSuccessful) return Redirect(returnUrl ?? "/admin");

        var userResult = await userAuth.Authenticate(Email, Password);
        if (userResult.IsSuccessful) return Redirect(returnUrl ?? "/");

        if (userResult.FailureMessage != null) ModelState.AddModelError(string.Empty, userResult.FailureMessage);

        return Page();
    }
}