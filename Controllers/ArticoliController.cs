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

        public ActionResult Index()
        {
            var articoli = dbContext.Articoli.ToList();
            return View(articoli);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Articoli articolo)
        {
            if (ModelState.IsValid)
            {
                dbContext.Articoli.Add(articolo);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articolo);
        }

        public ActionResult Edit(int id)
        {
            var articolo = dbContext.Articoli.Find(id);
            return View(articolo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Articoli articolo)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(articolo).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articolo);
        }

        public ActionResult Delete(int id)
        {
            var articolo = dbContext.Articoli.Find(id);
            return View(articolo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var articolo = dbContext.Articoli.Find(id);
            dbContext.Articoli.Remove(articolo);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
