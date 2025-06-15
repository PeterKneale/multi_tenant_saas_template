using Core.Application.Auth.Exceptions;
using Core.Application.Auth.Queue;
using Core.Application.Contracts;
using Core.Application.Organisations.Exceptions;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Contracts;
using Core.Domain.Organisations.Specifications;
using Core.Domain.Users;
using Core.Domain.Users.Contracts;
using Core.Domain.Users.Specifications;

namespace Core.Application.Auth.Commands;

public static class Register
{
    [SensitiveData]
    public record Command(
        Guid OrganisationId,
        string Title,
        Guid UserId,
        string FirstName,
        string LastName,
        string Email,
        string Password) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.OrganisationId).NotEmpty();
            RuleFor(m => m.Title).NotEmpty().MaximumLength(OrganisationConstants.MaxNameLength);
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(UserConstants.MaxFirstNameLength);
            RuleFor(m => m.LastName).NotEmpty().MaximumLength(UserConstants.MaxLastNameLength);
            RuleFor(m => m.Email).NotEmpty().MaximumLength(UserConstants.MaxEmailLength).EmailAddress();
            RuleFor(m => m.Password).NotEmpty().MaximumLength(UserConstants.MaxPasswordLength);
        }
    }

    public class Handler(
        IRepository<Organisation> organisations,
        IRepository<User> users,
        IOrganisationNameCheck check,
        IPasswordHash hash,
        ICommandQueue queue,
        ILogger<Handler> log)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            log.LogInformation("Registering organisation {OrganisationTitle} with owner {Email}", command.Title,
                command.Email);

            var organisationId = new OrganisationId(command.OrganisationId);
            var name = new OrganisationName(command.Title);

            var userId = new UserId(command.UserId);
            var userName = new Name(command.FirstName, command.LastName);
            var email = EmailAddress.Create(command.Email);
            var password = command.Password;

            if (await users.AnyAsync(new GetUserByEmailGlobalSpecification(email), cancellationToken))
                throw new EmailAlreadyExistsException(email);

            if (await users.AnyAsync(new GetUserByIdGlobalSpecification(userId), cancellationToken))
                throw new UserAlreadyExistsException(userId);

            if (await organisations.AnyAsync(new GetOrganisationByIdGlobally(organisationId), cancellationToken))
                throw new OrganisationAlreadyExistsException(organisationId);

            var organisation = Organisation.Create(organisationId, name, check);
            var user = User.CreateOwner(organisationId, userId, userName, email, password, hash);

            await organisations.AddAsync(organisation, cancellationToken);
            await users.AddAsync(user, cancellationToken);

            await queue.QueueHighPriorityCommand(new SendVerifyEmailAddressEmail.Command(user.Id), cancellationToken);
        }
    }
}