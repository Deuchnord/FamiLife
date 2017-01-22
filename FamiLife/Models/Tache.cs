using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace FamiLife.Models
{
    public class Tache
    {
        [Key]
        public int id { get; set; }
        [Required]
        public String titre { get; set; }
        public String description { get; set; }
        public DateTime echeance { get; set; }
        public bool tacheFaite { get; set; }
        public bool valideeParParents { get; set; }
        public int donneeParID { get; set; }
        public virtual Utilisateur donneePar { get; set; }

        public virtual ICollection<Utilisateur> donneeA { get; set; }
    
    }

   
}