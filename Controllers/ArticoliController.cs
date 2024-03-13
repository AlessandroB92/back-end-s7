using back_end_s7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace back_end_s7.Controllers
{
    public class ArticoliController : Controller
    {
        private FornoDbContext dbContext = new FornoDbContext();

        // GET: Articoli
        public ActionResult Index()
        {
            // Recupera tutti gli articoli dal database
            var articoli = dbContext.Articoli.ToList();
            return View(articoli);
        }

        // GET: Articoli/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articoli/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Articoli articolo)
        {
            if (ModelState.IsValid)
            {
                // Aggiungi il nuovo articolo al database
                dbContext.Articoli.Add(articolo);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articolo);
        }

        // GET: Articoli/Edit/5
        public ActionResult Edit(int id)
        {
            // Recupera l'articolo dal database
            var articolo = dbContext.Articoli.Find(id);
            return View(articolo);
        }

        // POST: Articoli/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Articoli articolo)
        {
            if (ModelState.IsValid)
            {
                // Aggiorna l'articolo nel database
                dbContext.Entry(articolo).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articolo);
        }

        // GET: Articoli/Delete/5
        public ActionResult Delete(int id)
        {
            // Recupera l'articolo dal database
            var articolo = dbContext.Articoli.Find(id);
            return View(articolo);
        }

        // POST: Articoli/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Trova e rimuovi l'articolo dal database
            var articolo = dbContext.Articoli.Find(id);
            dbContext.Articoli.Remove(articolo);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
