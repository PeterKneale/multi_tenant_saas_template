using Core.Application.Auth.Commands;
using static Core.Domain.Users.UserConstants;

namespace Web.Pages.Auth;

public class Forgot(IMediator mediator) : PageModel
{
    [Display(Name = "Email")]
    [Required]
    [BindProperty]
    [StringLength(MaxEmailLength)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await mediator.Send(new ForgotPassword.Command(Email));
            TempData.AddAlert(Alert.Success("Your password reset has been requested, please check your email"));
            return Page();
        }
        catch (BusinessRuleBrokenException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }
}