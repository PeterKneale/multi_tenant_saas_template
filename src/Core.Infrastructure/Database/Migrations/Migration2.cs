namespace Core.Infrastructure.Database.Migrations;

[Migration(2)]
public class Migration2 : Migration
{
    private const string TableName = "data_protection_keys";

    public override void Up()
    {
        Create.Table(TableName)
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("friendly_name").AsString()
            .WithColumn("xml").AsString();
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}