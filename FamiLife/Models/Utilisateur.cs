using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace FamiLife.Models
{
    public class Utilisateur
    {
        public int id { get; set; }
       [Required] public String nom { get; set; }
       [Required] public String prenom { get; set; }
       [Required] public String password { get; set; }
        public String surnom { get; set; }
        public int roleID { get; set; }
        public virtual Role role { get; set; }
        public virtual ICollection<Tache> taches { get; set; }
    }

}