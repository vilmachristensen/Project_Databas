using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Project_Databas.Models
{
    public class KundkorgDetaljer
    {
        public KundkorgDetaljer()
        {
        }
        [Display(Name = "Köpare")]
        public string Pr_Namn { get; set; }

        [Display(Name = "Produkt")]
        public string Prd_Namn { get; set; }

        [Display(Name = "Pris")]
        public int Prd_Pris { get; set; }
    }
}

