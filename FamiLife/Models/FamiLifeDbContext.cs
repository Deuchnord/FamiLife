using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FamiLife.Models
{
    public class FamiLifeDbContext : DbContext
    {
        public System.Data.Entity.DbSet<FamiLife.Models.Utilisateur> Utilisateurs { get; set; }
        public System.Data.Entity.DbSet<FamiLife.Models.Role> Roles { get; set; }
        public System.Data.Entity.DbSet<FamiLife.Models.Tache> Taches { get; set; }
    }
}