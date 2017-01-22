using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FamiLife.Models
{
    public class FamiLifeDbContext : DbContext
    {
        public System.Data.Entity.DbSet<FamiLife.Models.Utilisateur> Utilisateurs { get; set; }
        public System.Data.Entity.DbSet<FamiLife.Models.Role> Roles { get; set; }
        public System.Data.Entity.DbSet<FamiLife.Models.Tache> Taches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Tache>()
                .HasMany(t => t.donneeA).WithMany(u => u.taches)
                .Map(t => t.MapLeftKey("TacheID")
                .MapRightKey("UtilisateurID")
                .ToTable("Tache_Utilisateur"));
        }
    }
}