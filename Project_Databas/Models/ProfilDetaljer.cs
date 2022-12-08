using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Project_Databas.Models
{
    public class ProfilDetaljer
    {
        public ProfilDetaljer() { }

        [Display(Name = "Id")]
        public int Pr_Id { get; set; }

        [Display(Name = "För och efternamn")]
        public string Pr_Namn { get; set; }

        [Display(Name = "Lösenord")]
        public string Pr_Losenord { get; set; }

        [Display(Name = "Mailadress")]
        public string Pr_Mail { get; set; }

        [Display(Name = "Län")]
        public int Pr_Bor { get; set; }

        [Display(Name = "Ladda upp profilbild")]
        public IFormFile ImageFile { get; set; }
    }

}



 

