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
        [Required(ErrorMessage = "Fyll i för och efternamn")]
        public string Pr_Namn { get; set; }

        [Display(Name = "Lösenord")]
        [MinLength(8, ErrorMessage = "Lösenordet måste innehålla minst 8 tecken")]
        [Required(ErrorMessage = "Välj lösenord")]
        public string Pr_Losenord { get; set; }

        [Display(Name = "Mailadress")]
        [Required(ErrorMessage = "Fyll i mailadress")]
        public string Pr_Mail { get; set; }

        [Display(Name = "Län")]
        [Required(ErrorMessage = "Fyll i länets Id")]
        public int Pr_Bor { get; set; }

        [Display(Name = "Ladda upp profilbild")]
        public IFormFile ImageFile { get; set; }
    }

}



 

