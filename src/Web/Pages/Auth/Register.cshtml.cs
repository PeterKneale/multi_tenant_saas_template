using Core.Application.Auth.Commands;
using Core.Domain.Organisations;
using static Core.Domain.Users.UserConstants;

namespace Web.Pages.Auth;

public class RegisterPage(IMediator mediator, ILogger<RegisterPage> logs) : PageModel
{
    [Display(Name = "Organisation Name")]
    [Required]
    [BindProperty]
    [StringLength(OrganisationConstants.MaxNameLength)]
    public string Organisation { get; set; }

    [Display(Name = "First Name")]
    [Required]
    [BindProperty]
    [StringLength(MaxFirstNameLength)]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    [Required]
    [BindProperty]
    [StringLength(MaxLastNameLength)]
    public string LastName { get; set; }

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
    ///     This field is a honeypot to prevent bots from registering
    /// </summary>
    [BindProperty]
    [StringLength(100)]
    public string? Name { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // If the honeypot field is filled in, then the user is a bot
        if (Name != null)
        {
            logs.LogWarning("Bot detected entering : {Value} in honeypot field", Name);
            TempData.AddAlert(Alert.Danger("Bot detected"));
            return Page();
        }

        try
        {
            await mediator.Send(new Register.Command(Guid.NewGuid(), Organisation, Guid.NewGuid(), FirstName, LastName, Email, Password));
            TempData.AddAlert(Alert.Success("Please check your email in order to verify your account"));
            return RedirectToPage(nameof(Registered));
        }
        catch (BusinessRuleBrokenException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }
}