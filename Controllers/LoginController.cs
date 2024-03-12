using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using back_end_s7.Models;

namespace back_end_s7.Controllers
{
    public class LoginController : Controller
    {
        private FornoDbContext dbContext = new FornoDbContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Logged");
            }
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Amministratori admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var authenticatedAdmin = dbContext.Amministratori
                        .FirstOrDefault(a => a.Username == admin.Username && a.Password == admin.Password && a.Ruolo == "Admin");

                    if (authenticatedAdmin != null)
                    {
                        FormsAuthentication.SetAuthCookie(authenticatedAdmin.ID.ToString(), true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Credenziali errate. Riprova.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Si è verificato un errore durante il login.");
                    return View(admin);
                }
            }
            return View(admin);
        }

        [Authorize]
        public ActionResult Logged()
        {
            var adminID = HttpContext.User.Identity.Name;
            ViewBag.AdminID = adminID;
            return View();
        }

        [Authorize, HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
