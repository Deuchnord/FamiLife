namespace FamiLife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        libelle = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Taches",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        titre = c.String(nullable: false),
                        description = c.String(),
                        echeance = c.DateTime(nullable: false),
                        tacheFaite = c.Boolean(nullable: false),
                        valideeParParents = c.Boolean(nullable: false),
                        Utilisateur_id = c.Int(),
                        donneePar_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Utilisateurs", t => t.Utilisateur_id)
                .ForeignKey("dbo.Utilisateurs", t => t.donneePar_id, cascadeDelete: true)
                .Index(t => t.Utilisateur_id)
                .Index(t => t.donneePar_id);
            
            CreateTable(
                "dbo.Utilisateurs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nom = c.String(nullable: false),
                        prenom = c.String(nullable: false),
                        surnom = c.String(),
                        RoleID = c.Int(nullable: false),
                        Tache_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .ForeignKey("dbo.Taches", t => t.Tache_id)
                .Index(t => t.RoleID)
                .Index(t => t.Tache_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Taches", "donneePar_id", "dbo.Utilisateurs");
            DropForeignKey("dbo.Utilisateurs", "Tache_id", "dbo.Taches");
            DropForeignKey("dbo.Taches", "Utilisateur_id", "dbo.Utilisateurs");
            DropForeignKey("dbo.Utilisateurs", "RoleID", "dbo.Roles");
            DropIndex("dbo.Utilisateurs", new[] { "Tache_id" });
            DropIndex("dbo.Utilisateurs", new[] { "RoleID" });
            DropIndex("dbo.Taches", new[] { "donneePar_id" });
            DropIndex("dbo.Taches", new[] { "Utilisateur_id" });
            DropTable("dbo.Utilisateurs");
            DropTable("dbo.Taches");
            DropTable("dbo.Roles");
        }
    }
}
