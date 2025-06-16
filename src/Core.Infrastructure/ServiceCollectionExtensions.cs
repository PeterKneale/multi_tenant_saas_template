using System.Reflection;
using Core.Application.Contracts;
using Core.Domain.Invitations.Contracts;
using Core.Domain.Organisations.Contracts;
using Core.Domain.Users.Contracts;
using Core.Infrastructure.Behaviours;
using Core.Infrastructure.Database;
using Core.Infrastructure.Database.Repository;
using Core.Infrastructure.Emails;
using Core.Infrastructure.Services;
using Microsoft.AspNetCore.DataProtection;

namespace Core.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDbConnectionString();
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string missing");

        // Mediatr
        services.AddMediatR(config =>
        {
            var assembly = Assembly.GetExecutingAssembly();
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            config.AddOpenBehavior(typeof(TransactionalBehaviour<,>));
        });


        // repositories
        services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // queues
        services.AddScoped<ICommandQueue, CommandQueue>();

        // domain events
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        // services
        services.AddScoped<IOrganisationNameCheck, OrganisationNameCheck>();
        services.AddScoped<IInvitationEmailCheck, InvitationEmailCheck>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<IPasswordCheck, PasswordService>();
        services.AddSingleton<IPasswordHash, PasswordService>();


        // unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString, opt => opt.EnableRetryOnFailure());
            options.EnableDetailedErrors(false);
            options.EnableSensitiveDataLogging(false);
            options.UseSnakeCaseNamingConvention();
        });

        // migrations
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

        // emails
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<EmailBuilder>();
        services.AddScoped<EmailSender>();
        
        var emailBuilderOptions = configuration
            .GetSection(EmailBuilderOptions.SectionName)
            .Get<EmailBuilderOptions>() ?? throw new InvalidOperationException($"Configuration section '{EmailBuilderOptions.SectionName}' is missing or invalid.");
        var emailSenderOptions = configuration
            .GetSection(EmailSenderOptions.SectionName)
            .Get<EmailSenderOptions>() ?? throw new InvalidOperationException($"Configuration section '{EmailSenderOptions.SectionName}' is missing or invalid.");
        services.AddSingleton(emailBuilderOptions);
        services.AddSingleton(emailSenderOptions);
        
        // cache
        services.AddMemoryCache();

        // https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-8.0
        services.AddDataProtection().PersistKeysToDbContext<DatabaseContext>();
        return services;
    }
}