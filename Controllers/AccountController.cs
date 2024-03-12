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

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(new LoginUtenti());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUtenti loginModel)
        {
            if (ModelState.IsValid)
            {
                var utente = dbContext.Utenti.FirstOrDefault(u => u.Username == loginModel.Username && u.Password == loginModel.Password);
                if (utente != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Nome utente o password non validi.");
                }
            }

            return View(loginModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}