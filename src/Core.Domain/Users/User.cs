using Core.Domain.Users.Contracts;
using Core.Domain.Users.Rules;

namespace Core.Domain.Users;

public class User : BaseEntity
{
    private User()
    {
    }

    public User(OrganisationId organisationId, UserId id, Name name, UserRole role, EmailAddress email, string password)
    {
        OrganisationId = organisationId;
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Role = role;
        Active = null;
        Verified = false;
        VerifiedToken = Guid.NewGuid().ToString();
        CreatedAt = SystemTime.UtcNow();
    }

    public OrganisationId OrganisationId { get; private set; }

    public virtual UserId Id { get; private init; }

    public Name Name { get; private set; }

    public bool? Active { get; private set; }

    public bool Verified { get; private set; }

    public string? VerifiedToken { get; private set; }

    public string? ForgottenToken { get; set; }

    public DateTimeOffset? ForgottenTokenExpiry { get; set; }

    public EmailAddress Email { get; private set; }

    public string Password { get; private set; }

    public UserRole Role { get; private set; }

    public DateTimeOffset CreatedAt { get; private init; }

    public static User CreateMember(OrganisationId organisationId, UserId userId, Name name, EmailAddress email,
        string password, IPasswordHash hasher)
    {
        return new User(organisationId, userId, name, UserRole.Member, email, hasher.HashPassword(password));
    }

    public static User CreateOwner(OrganisationId organisationId, UserId userId, Name name, EmailAddress email,
        string password, IPasswordHash hasher)
    {
        return new User(organisationId, userId, name, UserRole.Owner, email, hasher.HashPassword(password));
    }

    public bool CanUserLogin(string password, IPasswordCheck checker)
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new MustBeActiveRule(this));
        return checker.Matches(password, Password);
    }

    public void ForgotPassword()
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new MustBeActiveRule(this));
        ForgottenToken = Guid.NewGuid().ToString();
        ForgottenTokenExpiry = SystemTime.UtcNow() + TimeSpan.FromHours(24);
    }

    public void ResetPassword(string token, string password, IPasswordHash hasher)
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new MustBeActiveRule(this));
        CheckRule(new MustHaveForgotPasswordRule(this));
        CheckRule(new ForgotTokenMustMatchRule(this, token));
        CheckRule(new ForgotTokenMustNotBeExpiredRule(this));
        ForgottenToken = null;
        ForgottenTokenExpiry = null;
        Password = hasher.HashPassword(password);
    }

    public void SetRole(UserRole role)
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new MustBeActiveRule(this));
        Role = role;
    }

    public void VerifyAndActivate(string verification)
    {
        CheckRule(new MustNotBeVerifiedRule(this));
        CheckRule(new VerificationTokenMustMatchRule(this, verification));
        Verified = true;
        VerifiedToken = null;
        Active = true;
    }

    public void Activate()
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new MustNotBeActiveRule(this));
        Active = true;
    }

    public void Deactivate(IUserContext context)
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new MustNotBeSelfRule(this, context));
        CheckRule(new MustBeActiveRule(this));
        Active = false;
    }

    public void ChangeName(Name name)
    {
        Name = name;
    }

    public void ChangePassword(string oldPassword, string newPassword, IPasswordCheck checker, IPasswordHash hasher)
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new MustBeActiveRule(this));
        CheckRule(new PasswordMustMatchRule(this, oldPassword, checker));
        Password = hasher.HashPassword(newPassword);
    }

    public void ChangeOrganisation(OrganisationId organisationId)
    {
        OrganisationId = organisationId;
    }
}