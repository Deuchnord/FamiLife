namespace FamiLife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databaseForRoleIdInUtilisateurs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Utilisateurs", "role_Id", "dbo.Roles");
            DropIndex("dbo.Utilisateurs", new[] { "role_Id" });
            RenameColumn(table: "dbo.Utilisateurs", name: "role_Id", newName: "roleID");
            AlterColumn("dbo.Utilisateurs", "roleID", c => c.Int(nullable: false));
            CreateIndex("dbo.Utilisateurs", "roleID");
            AddForeignKey("dbo.Utilisateurs", "roleID", "dbo.Roles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Utilisateurs", "roleID", "dbo.Roles");
            DropIndex("dbo.Utilisateurs", new[] { "roleID" });
            AlterColumn("dbo.Utilisateurs", "roleID", c => c.Int());
            RenameColumn(table: "dbo.Utilisateurs", name: "roleID", newName: "role_Id");
            CreateIndex("dbo.Utilisateurs", "role_Id");
            AddForeignKey("dbo.Utilisateurs", "role_Id", "dbo.Roles", "Id");
        }
    }
}
