using System;
using System.Linq;
using System.Web.Mvc;
using back_end_s7.Models;

namespace back_end_s7.Controllers
{
    public class HomeController : Controller
    {
        private readonly FornoDbContext dbContext = new FornoDbContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AggiungiAlCarrello(int idArticolo, int quantita)
        {
            string username = User.Identity.Name;
            Utenti utente = dbContext.Utenti.FirstOrDefault(u => u.Username == username);
            if (utente != null)
            {
                // Cerca l'ordine dell'utente in corso o creane uno nuovo se non esiste
                Ordini ordine = dbContext.Ordini.FirstOrDefault(o => o.IDUtente == utente.ID && o.Stato == "Elaborazione");
                if (ordine == null)
                {
                    ordine = new Ordini
                    {
                        IDUtente = utente.ID,
                        DataOrdine = DateTime.Now,
                        Stato = "Elaborazione",
                        Indirizzo = "Indirizzo di default", // Puoi sostituire con l'indirizzo dell'utente
                        Totale = 0 // Inizialmente impostato a 0, verrà calcolato più avanti
                    };
                    dbContext.Ordini.Add(ordine);
                    dbContext.SaveChanges();
                }

                // Aggiungi il dettaglio dell'ordine per l'articolo aggiunto al carrello
                Articoli articolo = dbContext.Articoli.FirstOrDefault(a => a.ID == idArticolo);
                if (articolo != null)
                {
                    decimal prezzoTotale = quantita * articolo.Prezzo;
                    OrdiniArticoli dettaglio = new OrdiniArticoli
                    {
                        IDOrdine = ordine.ID,
                        IDArticolo = articolo.ID,
                        Quantita = quantita
                    };
                    dbContext.OrdiniArticoli.Add(dettaglio);
                    dbContext.SaveChanges();
                    TempData["Message"] = "Articolo aggiunto al carrello con successo.";

                    // Aggiorna il totale dell'ordine
                    ordine.Totale += prezzoTotale;
                    dbContext.SaveChanges();

                    return RedirectToAction("Index", "Articoli");
                }
            }

            TempData["ErrorMessage"] = "Per aggiungere articoli al carrello, devi essere autenticato.";
            return RedirectToAction("Index", "Login");
        }

        public ActionResult RiepilogoOrdine()
        {
            string username = User.Identity.Name;
            Utenti utente = dbContext.Utenti.FirstOrDefault(u => u.Username == username);
            if (utente != null)
            {
                Ordini ordine = dbContext.Ordini.FirstOrDefault(o => o.IDUtente == utente.ID && o.Stato == "Elaborazione");
                if (ordine != null)
                {
                    var dettagliOrdine = dbContext.OrdiniArticoli.Include("Articoli").Where(d => d.IDOrdine == ordine.ID).ToList();
                    ViewBag.Ordine = ordine;
                    ViewBag.DettagliOrdine = dettagliOrdine;
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificaOrdine(string indirizzo, string note)
        {
            string username = User.Identity.Name;
            Utenti utente = dbContext.Utenti.FirstOrDefault(u => u.Username == username);
            if (utente != null)
            {
                Ordini ordine = dbContext.Ordini.FirstOrDefault(o => o.IDUtente == utente.ID && o.Stato == "Elaborazione");
                if (ordine != null)
                {
                    // Aggiorna l'indirizzo di consegna e le note dell'ordine
                    ordine.Indirizzo = indirizzo;
                    ordine.Note = note;

                    dbContext.SaveChanges();

                    TempData["Message"] = "Modifiche all'ordine salvate con successo.";
                    return RedirectToAction("RiepilogoOrdine");
                }
            }

            TempData["ErrorMessage"] = "Si è verificato un errore durante il salvataggio delle modifiche all'ordine.";
            return RedirectToAction("RiepilogoOrdine");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaOrdine()
        {
            string username = User.Identity.Name;
            Utenti utente = dbContext.Utenti.FirstOrDefault(u => u.Username == username);
            if (utente != null)
            {
                Ordini ordine = dbContext.Ordini.FirstOrDefault(o => o.IDUtente == utente.ID && o.Stato == "Elaborazione");
                if (ordine != null)
                {
                    // Trova e elimina tutti i dettagli dell'ordine (OrdiniArticoli) associati all'ordine corrente
                    var dettagliOrdine = dbContext.OrdiniArticoli.Where(d => d.IDOrdine == ordine.ID).ToList();
                    foreach (var dettaglio in dettagliOrdine)
                    {
                        dbContext.OrdiniArticoli.Remove(dettaglio);
                    }

                    // Elimina l'ordine stesso
                    dbContext.Ordini.Remove(ordine);
                    dbContext.SaveChanges();

                    TempData["Message"] = "Ordine eliminato con successo.";
                    return RedirectToAction("RiepilogoOrdine", "Home");
                }
            }

            TempData["ErrorMessage"] = "Nessun ordine trovato da eliminare.";
            return RedirectToAction("RiepilogoOrdine", "Home");
        }


    }
}
