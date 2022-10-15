using FluentMigrator;

namespace Practices.FluentMigration.MIgrations;

[Migration(1)]
public class CreateTableEntities : Migration
{
    public override void Up()
    {
        Create.Table("entities")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("title").AsString(50).NotNullable()
            .WithColumn("description").AsString(500).NotNullable()
            .WithColumn("createdAt").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("entities");
    }
}