namespace Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdUser = c.Int(),
                        ClaimType = c.String(maxLength: 100),
                        ClaimValue = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProfilePermission",
                c => new
                    {
                        ProfileId = c.Int(nullable: false),
                        PermissionId = c.Int(nullable: false),
                        IdUser = c.Int(),
                    })
                .PrimaryKey(t => new { t.ProfileId, t.PermissionId })
                .ForeignKey("dbo.Permission", t => t.PermissionId)
                .ForeignKey("dbo.Profile", t => t.ProfileId)
                .Index(t => t.ProfileId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.Profile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Active = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfileId = c.Int(nullable: false),
                        Cpf = c.String(nullable: false, maxLength: 11),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Active = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        Username = c.String(nullable: false, maxLength: 50),
                        NickName = c.String(nullable: false, maxLength: 15),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(nullable: false),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(maxLength: 50),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUTC = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profile", t => t.ProfileId)
                .Index(t => t.ProfileId);
            
            CreateTable(
                "dbo.Talent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Cpf = c.String(),
                        ResumeUniqueName = c.String(nullable: false, maxLength: 200),
                        ResumeFileName = c.String(nullable: false, maxLength: 50),
                        Active = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UpdatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        IdUser = c.Int(nullable: false),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.IdUser, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.User", t => t.IdUser)
                .Index(t => t.IdUser);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfilePermission", "ProfileId", "dbo.Profile");
            DropForeignKey("dbo.UserLogin", "IdUser", "dbo.User");
            DropForeignKey("dbo.User", "ProfileId", "dbo.Profile");
            DropForeignKey("dbo.Talent", "UpdatedBy", "dbo.User");
            DropForeignKey("dbo.ProfilePermission", "PermissionId", "dbo.Permission");
            DropIndex("dbo.UserLogin", new[] { "IdUser" });
            DropIndex("dbo.Talent", new[] { "UpdatedBy" });
            DropIndex("dbo.User", new[] { "ProfileId" });
            DropIndex("dbo.ProfilePermission", new[] { "PermissionId" });
            DropIndex("dbo.ProfilePermission", new[] { "ProfileId" });
            DropTable("dbo.UserLogin");
            DropTable("dbo.Talent");
            DropTable("dbo.User");
            DropTable("dbo.Profile");
            DropTable("dbo.ProfilePermission");
            DropTable("dbo.Permission");
        }
    }
}
