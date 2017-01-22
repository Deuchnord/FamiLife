namespace FamiLife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databaseForDonneeParinTache : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Taches", name: "donneePar_id", newName: "donneeParID");
            RenameIndex(table: "dbo.Taches", name: "IX_donneePar_id", newName: "IX_donneeParID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Taches", name: "IX_donneeParID", newName: "IX_donneePar_id");
            RenameColumn(table: "dbo.Taches", name: "donneeParID", newName: "donneePar_id");
        }
    }
}
