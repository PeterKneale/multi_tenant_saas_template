using static Core.Infrastructure.Database.DbColumnNames;
using static Core.Infrastructure.Database.DbTableNames;

namespace Core.Infrastructure.Database.Migrations;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(Organisations)
            .WithColumn(OrganisationIdColumn).AsGuid().PrimaryKey()
            .WithColumn(Title).AsString(100).Unique()
            .WithColumn(Description).AsString(100).Nullable()
            .WithColumn(CreatedAt).AsDateTimeOffset().NotNullable();

        Create.Table(Users)
            .WithColumn(UserIdColumn).AsGuid().PrimaryKey()
            .WithColumn(OrganisationIdColumn).AsGuid()
            .WithColumn(FirstName).AsString(100)
            .WithColumn(LastName).AsString(100)
            .WithColumn(Email).AsString(200).Unique()
            .WithColumn(Password).AsString(100)
            .WithColumn(Active).AsBoolean().Nullable()
            .WithColumn(Verified).AsBoolean()
            .WithColumn(VerifiedToken).AsString(50).Nullable()
            .WithColumn(ForgotToken).AsString(50).Nullable()
            .WithColumn(ForgotTokenExpiry).AsDateTimeOffset().Nullable()
            .WithColumn(Role).AsString(50)
            .WithColumn(CreatedAt).AsDateTimeOffset().NotNullable();

        Create.ForeignKey($"fk_{Users}_{Organisations}")
            .FromTable(Users).ForeignColumn(OrganisationIdColumn)
            .ToTable(Organisations).PrimaryColumn(OrganisationIdColumn);

        Create.Table(Invitations)
            .WithColumn(InvitationIdColumn).AsGuid().PrimaryKey()
            .WithColumn(OrganisationIdColumn).AsGuid()
            .WithColumn(UserIdColumn).AsGuid()
            .WithColumn(Email).AsString().Unique()
            .WithColumn(CreatedAt).AsDateTimeOffset().NotNullable();

        Create.ForeignKey($"fk_{Invitations}_{Organisations}")
            .FromTable(Invitations).ForeignColumn(OrganisationIdColumn)
            .ToTable(Organisations).PrimaryColumn(OrganisationIdColumn);

        Create.ForeignKey($"fk_{Invitations}_{Users}")
            .FromTable(Invitations).ForeignColumn(UserIdColumn)
            .ToTable(Users).PrimaryColumn(UserIdColumn);
    }

    public override void Down()
    {
        DropTableIfExists(Invitations);
        DropTableIfExists(Users);
        DropTableIfExists(Organisations);
    }

    private void DropTableIfExists(string tableName)
    {
        Execute.Sql($"DROP TABLE IF EXISTS {tableName};");
    }
}