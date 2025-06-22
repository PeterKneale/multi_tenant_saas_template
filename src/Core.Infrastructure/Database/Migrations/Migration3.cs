using static Core.Infrastructure.Database.DbColumnNames;
using static Core.Infrastructure.Database.DbTableNames;

namespace Core.Infrastructure.Database.Migrations;

[Migration(3)]

public class Migration3 : Migration
{
    public override void Up()
    {
        Create.Table(Projects)
            .WithColumn(ProjectIdColumn).AsGuid().PrimaryKey()
            .WithColumn(OrganisationIdColumn).AsGuid()
            .WithColumn(Title).AsString(100).Unique()
            .WithColumn(Description).AsString(100).Nullable()
            .WithColumn(CreatedAt).AsDateTimeOffset().NotNullable()
            .WithColumn(CreatedBy).AsGuid().NotNullable();
        
        Create.ForeignKey($"fk_{Projects}_{Organisations}")
            .FromTable(Projects).ForeignColumn(OrganisationIdColumn)
            .ToTable(Organisations).PrimaryColumn(OrganisationIdColumn);
        
        Create.ForeignKey($"fk_{Projects}_{Users}")
            .FromTable(Projects).ForeignColumn(CreatedBy)
            .ToTable(Users).PrimaryColumn(UserIdColumn);
        
        Create.Table(Features)
            .WithColumn(FeatureIdColumn).AsGuid().PrimaryKey()
            .WithColumn(ProjectIdColumn).AsGuid()
            .WithColumn(OrganisationIdColumn).AsGuid()
            .WithColumn(Title).AsString(100).Unique()
            .WithColumn(Description).AsString(100).Nullable()
            .WithColumn(CreatedAt).AsDateTimeOffset().NotNullable()
            .WithColumn(CreatedBy).AsGuid().NotNullable();
        
        Create.ForeignKey($"fk_{Features}_{Projects}")
            .FromTable(Features).ForeignColumn(ProjectIdColumn)
            .ToTable(Projects).PrimaryColumn(ProjectIdColumn);
        
        Create.ForeignKey($"fk_{Features}_{Organisations}")
            .FromTable(Features).ForeignColumn(OrganisationIdColumn)
            .ToTable(Organisations).PrimaryColumn(OrganisationIdColumn);
        
        Create.ForeignKey($"fk_{Features}_{Users}")
            .FromTable(Features).ForeignColumn(CreatedBy)
            .ToTable(Users).PrimaryColumn(UserIdColumn);
    }

    public override void Down()
    {
        Delete.Table(Features);
        Delete.Table(Projects);
    }
}