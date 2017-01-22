namespace FamiLife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tentativeManyToMany2 : DbMigration
    {
        public override void Up()
        {
           
            DropForeignKey("dbo.Taches", "Utilisateur_id", "dbo.Utilisateurs");
            DropForeignKey("dbo.Utilisateurs", "Tache_id", "dbo.Taches");
            DropIndex("dbo.Taches", new[] { "Utilisateur_id" });
            DropIndex("dbo.Utilisateurs", new[] { "Tache_id" });
            CreateTable(
                "dbo.Tache_Utilisateur",
                c => new
                {
                    TacheID = c.Int(nullable: false),
                    UtilisateurID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.TacheID, t.UtilisateurID });
            AddForeignKey("dbo.Tache_Utilisateur", "TacheID", "dbo.Taches", "id", false);
            AddForeignKey("dbo.Tache_Utilisateur", "UtilisateurID", "dbo.Utilisateurs", "id", false);
            AddColumn("dbo.Utilisateurs", "password", c => c.String(nullable: false));
            DropColumn("dbo.Taches", "Utilisateur_id");
            DropColumn("dbo.Utilisateurs", "Tache_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Utilisateur", "Tache_id", c => c.Int());
            AddColumn("dbo.Tache", "Utilisateur_id", c => c.Int());
            DropForeignKey("dbo.Tache_Utilisateur", "UtilisateurID", "dbo.Utilisateur");
            DropForeignKey("dbo.Tache_Utilisateur", "TacheID", "dbo.Tache");
            DropIndex("dbo.Tache_Utilisateur", new[] { "UtilisateurID" });
            DropIndex("dbo.Tache_Utilisateur", new[] { "TacheID" });
            DropColumn("dbo.Utilisateur", "password");
            DropTable("dbo.Tache_Utilisateur");
            CreateIndex("dbo.Utilisateur", "Tache_id");
            CreateIndex("dbo.Tache", "Utilisateur_id");
            AddForeignKey("dbo.Utilisateurs", "Tache_id", "dbo.Taches", "id");
            AddForeignKey("dbo.Taches", "Utilisateur_id", "dbo.Utilisateurs", "id");
            RenameTable(name: "dbo.Utilisateur", newName: "Utilisateurs");
            RenameTable(name: "dbo.Tache", newName: "Taches");
            RenameTable(name: "dbo.Role", newName: "Roles");
        }
    }
}
