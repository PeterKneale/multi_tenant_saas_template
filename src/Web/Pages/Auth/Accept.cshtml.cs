using Core.Application.Invitations.Commands;
using Core.Application.Invitations.Queries;
using static Core.Domain.Users.UserConstants;

namespace Web.Pages.Auth;

public class Accept(IMediator mediator) : PageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid InvitationId { get; set; }

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

    public async Task<IActionResult> OnGetAsync()
    {
        var exists = await mediator.Send(new InvitationExists.Query(InvitationId));
        if (exists)
        {
            Email = await mediator.Send(new GetInviteeEmailAddress.Query(InvitationId));
            return Page();
        }

        TempData.AddAlert(Alert.Warning("The invitation you have selected has either been accepted or cancelled"));
        return RedirectToPage("/Auth/Login");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await mediator.Send(new AcceptInvitation.Command(InvitationId, Guid.NewGuid(), FirstName, LastName, Email,
                Password));
            TempData.AddAlert(Alert.Success(
                "Your invitation has been accepted and your account has been created, please login to continue"));
            return Redirect(nameof(Login));
        }
        catch (BusinessRuleBrokenException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }
}