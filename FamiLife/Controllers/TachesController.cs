using System;
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
            //ViewBag.Session.Utilisateur = Session["utilisateur"];
            return View(db.Taches.ToList());
        }

        public ActionResult MesTaches()
        {
            if(Session["utilisateur"] ==null && ((Utilisateur)Session["utilisateur"]).roleID !=2)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var tacheChildren = (from t in db.Taches
                                     where db.tach)
                                     select t).ToList();
                return View(tacheChildren);
            }
        }

        // GET: Taches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tache tache = db.Taches.Find(id);
            if (tache == null)
            {
                return HttpNotFound();
            }
            return View(tache);
        }

        // GET: Taches/Create
        public ActionResult Create()
        {
            var tache = new Tache();
            tache.donneeA = new List<Utilisateur>();
            getRoleDropdownList();
            getChildrenList(tache);
            return View();
        }

        // POST: Taches/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,titre,description,echeance,tacheFaite,valideeParParents,donneeParID,donneeA")] Tache tache,string[] selectedChildren)
        {
            if(selectedChildren !=null)
            {
                tache.donneeA = new List<Utilisateur>();
                foreach(var child in selectedChildren)
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
                catch(Exception e)
                {
                    
                }
                return RedirectToAction("Index");
            }

            getRoleDropdownList(tache.donneeParID);
            return View(tache);
        }

        // GET: Taches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tache tache = db.Taches.Find(id);
            if (tache == null)
            {
                return HttpNotFound();
            }
            return View(tache);
        }

        // POST: Taches/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,titre,description,echeance,tacheFaite,valideeParParents")] Tache tache)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tache).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tache);
        }

        // GET: Taches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tache tache = db.Taches.Find(id);
            if (tache == null)
            {
                return HttpNotFound();
            }
            return View(tache);
        }

        // POST: Taches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tache tache = db.Taches.Find(id);
            db.Taches.Remove(tache);
            db.SaveChanges();
            return RedirectToAction("Index");
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
