﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bumblebee.Setup;
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
            if (TempData["success"] != null)
            {
                ViewBag.s = "Ditt lösenord är skickat till din mail";
                TempData.Remove("success");
            }

            HttpContext.Session.Remove("session");
            HttpContext.Session.Remove("inloggatId");

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
                string s = pd.Pr_Mail;
                HttpContext.Session.SetString("session", s);

                int s2 = pd.Pr_Id;
                HttpContext.Session.SetInt32("inloggatId", s2);
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
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;

            if(s != null)
            {
                int s2 = (int)HttpContext.Session.GetInt32("inloggatId");

                ProfilDetaljer pd = new ProfilDetaljer();
                ProfilMetod pm = new ProfilMetod();
                pd = pm.GetInloggedProfil(s2, out string error);

                return RedirectToAction("Detaljer", pd);
            }
            else
            {
                return RedirectToAction("Inloggning");
            }
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

            if (i == 1) {
                string s = pd.Pr_Mail;
                HttpContext.Session.SetString("session", s);

                pd = pm.GetProfil(pd.Pr_Mail, pd.Pr_Losenord, out string errormsg);

                int s2 = pd.Pr_Id;
                HttpContext.Session.SetInt32("inloggatId", s2);
                return RedirectToAction("MinProfil", pd);
            }
            else return View("SkapaKonto");
        }


        // VISA DETALJER PROFIL
        public IActionResult Detaljer(string Pr_Mail, string Pr_Losenord)
        {
            ProfilDetaljer pd = new ProfilDetaljer();

            int s2 = (int)HttpContext.Session.GetInt32("inloggatId");

            ProfilMetod pm = new ProfilMetod();
            pd = pm.GetInloggedProfil(s2, out string error);

            BildMetoder bm = new BildMetoder();

            ViewBag.picture = null;
            if (bm.ViewPicture(out string errormsg, s2) != null)
            {
                Byte[] bytes = bm.ViewPicture(out string errormsg2,s2);

                ViewBag.picture = ViewImage(bytes);
            }

            return View(pd);
        }

        // REDIGERA PROFIL

        [HttpGet]
        public IActionResult Redigera(string Pr_Mail, string Pr_Losenord)
        {
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;

            ProfilDetaljer pd = new ProfilDetaljer();
            ProfilMetod pm = new ProfilMetod();
            pd = pm.GetProfil(Pr_Mail, Pr_Losenord, out string error);

            return View(pd);
        }

        [HttpPost]
        public IActionResult Redigera(ProfilDetaljer pd)
        {
            
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;

            int s2 = (int)HttpContext.Session.GetInt32("inloggatId");

            ProfilMetod pm = new ProfilMetod();
            int i = 0;
            string error = "";
            i = pm.EditProfil(pd, out error);

            if(pd.Pr_Bild != null)
            {
                BildMetoder bm = new BildMetoder();
                Byte[] bytes = bm.Upload(out string errormsg, pd, s2);

                var stream = new MemoryStream(bytes);
                IFormFile img = new FormFile(stream, 0, bytes.Length, "name", "fileName");
                pd.Pr_Bild = img;
            }

            return RedirectToAction("Detaljer", pd);
        }

        // VISA BILD
        [NonAction]
        private string ViewImage(byte[] arrayImage)
        {
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;
        }



        // TA BORT PROFIL
        public IActionResult Radera(int id)
        {
            ProfilMetod pm = new ProfilMetod();
            string error = "";
            int i = 0;
            i = pm.DeleteProfil(id, out error);
            return RedirectToAction("Inloggning");
        }

        // GLÖMT LÖSENORD
        [HttpGet]
        public IActionResult Glomt()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Glomt(string Pr_Mail)
        {
            string s;
            ProfilMetod pm = new ProfilMetod();

            if (pm.SendMail(Pr_Mail, out string errormsg)) {

                TempData["success"] = "Ditt lösenord är skickat till din mail";
                return RedirectToAction("Inloggning");
            }
            else
            {
                s = "Ange en giltig mail";
                ViewBag.s = s;
                return View("Glomt");
            }
        }

        // SHOP
        public IActionResult Shop()
        {
            List<ProduktDetaljer> ProduktLista = new List<ProduktDetaljer>();
            ProduktMetod pm = new ProduktMetod();
            ProduktLista = pm.GetProdukter(out string error);

            return View(ProduktLista);
        }

        // VISA MER, PRODUKT
        public IActionResult Item(int id)
        {
            ProduktDetaljer pd = new ProduktDetaljer();
            ProduktMetod pm = new ProduktMetod();

            pd = pm.GetProdukt(id, out string error);
            return View(pd);
        }

        // KUNDKORGEN
        public IActionResult Cart(int id)
        {
            string sIdInt = HttpContext.Session.GetString("inloggatId");

            if (sIdInt != null)
            {
                int s2 = (int)HttpContext.Session.GetInt32("inloggatId");

                ProduktMetod pm = new ProduktMetod();
                ProduktDetaljer pd = new ProduktDetaljer();
                int i = 0;
                
                i = pm.InsertKundkorg(pd, s2, id, out string errormsg2);

                List<KundkorgDetaljer> ProduktLista = new List<KundkorgDetaljer>();
                ProduktLista = pm.GetKundkorg(s2, out string errormsg);
                return View(ProduktLista);

            }
            else
            {
                return RedirectToAction("Inloggning");
            } 
        }

        // TA BORT PRODUKT
        public IActionResult RaderaProdukt(int id)
        {
            ProduktMetod pm = new ProduktMetod();
            string error = "";
            int i = 0;
            i = pm.DeleteProdukt(id, out error);
            return RedirectToAction("Cart");
        }
    }
}

