using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project_Databas.Models;

namespace Project_Databas.Controllers
{
    public class ProfilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // INLOGGNING
        [HttpGet]
        public IActionResult Inloggning()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Inloggning(string Pr_Mail, string Pr_Losenord)
        {
            ProfilDetaljer pd = new ProfilDetaljer();
            ProfilMetod pm = new ProfilMetod();
            pd = pm.GetProfil(Pr_Mail, Pr_Losenord, out string errormsg);


            if (pd != null)
            {
                return RedirectToAction("MinProfil", pd);
            }
            else
            {
                string error = "Felaktig mail eller lösenord";
                ViewBag.error = error;
                return View("Inloggning");
            }
        }
        // MIN PROFIL
        public IActionResult MinProfil(string Pr_Mail, string Pr_Losenord)
        {
            ProfilDetaljer pd = new ProfilDetaljer();
            ProfilMetod pm = new ProfilMetod();
            pd = pm.GetProfil(Pr_Mail, Pr_Losenord, out string error);
            ViewBag.error = error;
            return View(pd);
        }

        // SKAPA KONTO
        [HttpGet]
        public IActionResult SkapaKonto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SkapaKonto(ProfilDetaljer pd)
        {
            ProfilMetod pm = new ProfilMetod();
            int i = 0;
            string error = "";
            i = pm.SkapaKonto(pd, out error);
            ViewBag.error = error;
            ViewBag.antal = i;

            if (i == 1) { return RedirectToAction("MinProfil", pd); }
            else return View("SkapaKonto");
        }


        // REDIGERA PROFIL
        [HttpGet]
        public IActionResult Redigera(string Pr_Mail, string Pr_Losenord)
        {
            ProfilDetaljer pd = new ProfilDetaljer();
            ProfilMetod pm = new ProfilMetod();
            pd = pm.GetProfil(Pr_Mail, Pr_Losenord, out string error);
            return View(pd);
        }

        [HttpPost]
        public IActionResult Redigera(ProfilDetaljer pd)
        {
            ProfilMetod pm = new ProfilMetod();
            int i = 0;
            string error = "";
            i = pm.EditProfil(pd, out error);
            return RedirectToAction("MinProfil", pd);
        }

        //VISA DETALJER PROFIL
        public IActionResult Detaljer(string Pr_Mail, string Pr_Losenord)
        {
            ProfilDetaljer pd = new ProfilDetaljer();
            ProfilMetod pm = new ProfilMetod();
            pd = pm.GetProfil(Pr_Mail, Pr_Losenord, out string error);

            BildMetoder bm = new BildMetoder();

            ViewBag.picture = null;
            if (bm.ViewPicture(Pr_Mail, Pr_Losenord, out string errormsg) != null)
            {
                Byte[] bytes = bm.ViewPicture(Pr_Mail, Pr_Losenord, out string errormsg2);

                ViewBag.picture = ViewImage(bytes);
            }

            return View(pd);
        }

        //LADDA UPP BILD

        [HttpGet]
        public IActionResult Uppladdning()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Uppladdning(ProfilDetaljer pd)
        {

            BildMetoder bm = new BildMetoder();

            Byte[] bytes = bm.Upload(out string errormsg, pd);


            var stream = new MemoryStream(bytes);
            IFormFile img = new FormFile(stream, 0, bytes.Length, "name", "fileName");
            pd.ImageFile = img;

            return RedirectToAction("Detaljer",pd);
        }

        [NonAction]
        private string ViewImage(byte[] arrayImage)
        {
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;
        }
    }
}

