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
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = dbContext.Utenti.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                var admin = dbContext.Amministratori.FirstOrDefault(a => a.Username == model.Username && a.Password == model.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.ID.ToString(), true);
                    return RedirectToAction("Index", "Home");
                }
                if (admin != null)
                {
                    FormsAuthentication.SetAuthCookie(admin.ID.ToString(), true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Credenziali Errate.";
                    return RedirectToAction("Index", "Login");
                }

                // Se né utente né amministratore sono stati trovati, mostra il messaggio di errore
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            TempData["Message"] = "Logout effettuato con successo.";
            return RedirectToAction("Index", "Home");
        }
    }
}
