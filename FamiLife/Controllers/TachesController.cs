﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FamiLife.Models;
using FamiLife.ViewModels;

namespace FamiLife.Controllers
{
    public class TachesController : Controller
    {
        private FamiLifeDbContext db = new FamiLifeDbContext();

        // GET: Taches
        public ActionResult Index()
        {
            if (UtilisateursController.isAuthenticated(this))
            {
                if (((Utilisateur)(Session["utilisateur"])).roleID == 1)
                    return View(db.Taches.ToList());
                else
                    return RedirectToAction("MesTaches");
            }
            else
                return Redirect("/");
        }

        public ActionResult MesTaches()
        {
            if(Session["utilisateur"] == null || ((Utilisateur)Session["utilisateur"]).roleID != 2)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Utilisateur utilisateur = (Utilisateur)Session["utilisateur"];
                IEnumerable<Tache> mesTaches = ((from t in db.Taches
                                     where (t.donneeA.Select(u => u.id).Contains(utilisateur.id))
                                     select new {
                                         t.id,
                                         t.description,
                                         t.donneeA,
                                         t.donneePar,
                                         t.donneeParID,
                                         t.echeance,
                                         t.tacheFaite,
                                         t.titre,
                                         t.valideeParParents
                                     })).AsEnumerable()
                                     .Select(t => new Tache
                                     {
                                         id = t.id,
                                         description = t.description,
                                         donneeA = t.donneeA,
                                         donneePar = t.donneePar,
                                         donneeParID = t.donneeParID,
                                         echeance = t.echeance,
                                         tacheFaite = t.tacheFaite,
                                         titre = t.titre,
                                         valideeParParents = t.valideeParParents
                                     }).ToList();

                return View(mesTaches);
            }
        }

        // GET: Taches/Done/5
        public ActionResult Done(int? id)
        {
            if(UtilisateursController.isAuthenticated(this))
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                Tache tache = db.Taches.Find(id);

                if (tache == null)
                    return HttpNotFound();

                //if (!tache.donneeA.Contains((Utilisateur) Session["utilisateur"]))
                    //return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                // On inverse la valeur de tacheFaite. Ceci permet également d'annuler l'action
                tache.tacheFaite = !tache.tacheFaite;
                db.Entry(tache).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("MesTaches");
            }

            return Redirect("/");
        }

        // GET: Taches/Validate/5
        public ActionResult Validate(int? id)
        {
            if (UtilisateursController.isAuthenticated(this) && ((Utilisateur) Session["utilisateur"]).roleID == 1)
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                Tache tache = db.Taches.Find(id);

                if (tache == null)
                    return HttpNotFound();

                tache.valideeParParents = true;
                db.Entry(tache).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return Redirect("/");
        }

        // GET: Taches/Create
        public ActionResult Create()
        {
            if (UtilisateursController.isAuthenticated(this))
            {
                var tache = new Tache();
                tache.donneeA = new List<Utilisateur>();
                getRoleDropdownList();
                getChildrenList(tache);
                return View();
            }
            else
                return Redirect("/");
        }

        // POST: Taches/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,titre,description,echeance,tacheFaite,valideeParParents,donneeParID,donneeA")] Tache tache,string[] selectedChildren)
        {
            if (UtilisateursController.isAuthenticated(this))
            {
                if (selectedChildren != null)
                {
                    tache.donneeA = new List<Utilisateur>();
                    foreach (var child in selectedChildren)
                    {
                        var childToAdd = db.Utilisateurs.Find(int.Parse(child));
                        tache.donneeA.Add(childToAdd);
                    }
                }

                if (ModelState.IsValid)
                {
                    db.Taches.Add(tache);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {

                    }
                    return RedirectToAction("Index");
                }

                getRoleDropdownList(tache.donneeParID);
                return View(tache);
            }
            else
                return Redirect("/");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void getRoleDropdownList(object selectedParent = null)
        {
            var roleQuery = from r in db.Utilisateurs
                            where r.roleID == 1
                            orderby r.id
                            select r;
            ViewBag.donneeParID = new SelectList(roleQuery, "id", "surnom", selectedParent);
        }

        private void getChildrenList(Tache tache = null)
        {
            var alluser = db.Utilisateurs;
            var tacheUtilisateur = new HashSet<int>();
            var viewModel = new List<TacheAssigneeA>();
            foreach(var user in alluser)
            {
                viewModel.Add(new TacheAssigneeA
                {
                    UtilisateurID = user.id,
                    Nom = user.nom +" "+ user.prenom,
                    assigne = tacheUtilisateur.Contains(user.id)

                });
            }
            ViewBag.users = viewModel;
        }
    }
}
