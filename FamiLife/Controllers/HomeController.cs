﻿using FamiLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamiLife.Controllers
{
    public class HomeController : Controller
    {
        private FamiLifeDbContext db = new FamiLifeDbContext();

        // GET: Home
        public ActionResult Index()
        {
            if (Session["utilisateur"] == null)
            {
                // Si l'utilisateur n'est pas authentifié, on affiche 
                if (TempData["AuthErr"] != null)
                    ViewBag.authErr = TempData["AuthErr"].ToString();

                return View();
            }
            else
            {
                // Sinon, on redirige vers la liste de tâches de l'utilisateur courant.
                return Redirect("/taches");
            }
        }

        // POST: identify
        [HttpPost]
        public ActionResult Identify(string prenom, string mdp)
        {
            try
            {
                Utilisateur Utilisateur = (from u in db.Utilisateurs
                                           where u.prenom == prenom
                                           where u.password == mdp
                                           select new
                                           {
                                               u.id,
                                               u.nom,
                                               u.prenom,
                                               u.surnom,
                                               u.roleID,
                                               u.role,
                                               u.taches
                                           }).AsEnumerable()
                                           .Select(u => new Utilisateur {
                                               id = u.id,
                                               nom = u.nom,
                                               prenom = u.prenom,
                                               password = null,
                                               surnom = u.surnom,
                                               roleID = u.roleID,
                                               role = u.role,
                                               taches = u.taches
                                           }).ToList()[0];
                Session["utilisateur"] = Utilisateur;
                if(((Utilisateur)(Session["utilisateur"])).roleID==1)
                {
                    return Redirect("/taches");
                }
                else if (((Utilisateur)(Session["utilisateur"])).roleID == 2)
                {
                    return RedirectToAction("MesTaches", "Taches");
                }
                else
                {
                    return Redirect("/Index");
                }




            } catch(ArgumentOutOfRangeException e)
            {
                TempData["authErr"] = "Nom ou mot de passe incorrect !";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult logOut()
        {
            if(Session["utilisateur"] != null)
            {
                Session.Abandon();
            }
            return RedirectToAction("Index");
        }
    }
}