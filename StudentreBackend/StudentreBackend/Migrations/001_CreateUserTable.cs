using FluentMigrator;
using System.Data;

namespace StudentreBackend.MigrationsScripts
{
    [Migration(1)]
    public class CreateUserTable : Migration
    {
        public override void Up()
        {
            //Create.Table("Users")
            //    .WithColumn("Id").AsInt64().PrimaryKey().Identity()  
            //    .WithColumn("PublicId").AsFixedLengthAnsiString(36)  
            //    .WithColumn("DateCreatedUtc").AsDateTime2().WithColumn("CreatedBy").AsInt64().WithColumn("DateModifiedUtc").AsDateTime2().Nullable().WithColumn("ModifiedBy").AsInt64().Nullable() 
            //    .WithColumn("DateDeletedUtc").AsDateTime2().Nullable().WithColumn("DeletedBy").AsInt64().Nullable() 
            //    .WithColumn("FirstName").AsString(150)
            //    .WithColumn("Surname").AsString(150)
            //    .WithColumn("Login").AsString(250)
            //    .WithColumn("PasswordHash").AsString(250).Nullable()
            //    .WithColumn("IsAdmin").AsBoolean();

            if (!Schema.Table("Role").Exists())
            { 
                Create.Table("Role")
                    .WithId()
                    .WithColumn("Value").AsString(32);
            }
            if (!Schema.Table("User").Exists())
            {
                Create.Table("User")
                    .WithId()
                    .WithPublicId()
                    .WithAuditable()
                    .WithSoftDelete()
                    .WithColumn("LastLogin").AsDateTime2().Nullable()
                    .WithColumn("Login").AsString(64).Unique()
                    .WithColumn("Password").AsString(256)
                    .WithColumn("FirstName").AsString(32)
                    .WithColumn("LastName").AsString(32)
                    .WithColumn("Email").AsString(64).Unique()
                    .WithColumn("Photo").AsString(512).Nullable()
                    .WithColumn("StudentId").AsString(32)
                    .WithColumn("FieldOfStudy").AsString(64)
                    .WithColumn("Term").AsString(32)
                    .WithColumn("College").AsString(128)
                    .WithColumn("Department").AsString(128)
                    .WithColumn("Status").AsInt32()
                    .WithColumn("RefreshToken").AsString(256).Nullable()
                    .WithColumn("RefreshTokenExpiryTime").AsDateTime2().Nullable()
                    .WithColumn("RoleId").AsInt64()
                        .ForeignKey("FK_User_RoleId", "Role", "Id")
                        .OnDeleteOrUpdate(Rule.Cascade);
            }
            if (!Schema.Table("Group").Exists())
            {
                Create.Table("Group")
                    .WithId()
                    .WithColumn("Name").AsString(64);
            }
            if (!Schema.Table("UserGroup").Exists())
            {
                Create.Table("UserGroup")
                    .WithId()
                    .WithColumn("UserId").AsInt64()
                        .ForeignKey("FK_User_UserId", "User", "Id")
                    .WithColumn("GroupId").AsInt64()
                        .ForeignKey("FK_Group_GroupId", "Group", "Id");
            }
        }

        public override void Down()
        {
            // zachowac dobą kolejnosc usuwania tak aby nie prowowac usuwac tabel z istniejaca zaleznoscia do innej
            if (Schema.Table("UserGroup").Exists())
            {
                Delete.Table("UserGroup");
            }
            if (Schema.Table("Users").Exists())
            {
                Delete.Table("Users");
            }
            if (Schema.Table("Role").Exists())
            {
                Delete.Table("Role");
            }
            if (Schema.Table("Group").Exists())
            {
                Delete.Table("Group");
            }
            
        }

        
    }
}