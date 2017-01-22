namespace FamiLife.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FamiLife.Models.FamiLifeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(FamiLife.Models.FamiLifeDbContext context)
        {
            var roles = new List<Role>
           {
               new Role {libelle = "Parent" },
               new Role {libelle = "Enfant" }
           };

            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur {nom = "VAN DE KADSYE", prenom ="Laurent", surnom="Papa", roleID=1 }
            };
        }
    }
}
