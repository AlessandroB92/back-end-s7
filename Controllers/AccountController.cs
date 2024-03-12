using back_end_s7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace back_end_s7.Controllers
{
    public class AccountController : Controller
    {
        private FornoDbContext dbContext = new FornoDbContext();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Utenti utente)
        {
            if (ModelState.IsValid)
            {
                dbContext.Utenti.Add(utente);
                dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(utente);
        }
    }
}