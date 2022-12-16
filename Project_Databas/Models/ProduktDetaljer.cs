using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Project_Databas.Models
{
    public class ProduktDetaljer
    {
        public ProduktDetaljer() { }

        [Display(Name = "Id")]
        public int Prd_Id { get; set; }

        [Display(Name = "Produktnamn")]
        public string Prd_Namn { get; set; }

        [Display(Name = "Pris")]
        public int Prd_Pris { get; set; }

        [Display(Name = "Produktbeskrivning")]
        public string Prd_Beskrivning { get; set; }
    }
}

