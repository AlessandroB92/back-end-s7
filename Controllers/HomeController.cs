using System;
using System.Data.Entity;
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
        public ActionResult InviaOrdine()
        {
            // Puoi implementare qui la logica per l'invio dell'ordine

            // Aggiungi un messaggio di conferma per l'utente
            TempData["Message"] = "Ordine inviato con successo.";

            // Reindirizza all'azione RiepilogoOrdine per visualizzare nuovamente l'ordine
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

        [Authorize(Roles = "Admin")]
        public ActionResult GestioneOrdini()
        {
            var ordini = dbContext.Ordini.Include("Utenti").Include("OrdiniArticoli.Articoli").ToList();
            return View(ordini);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult ModificaStatoOrdine(int idOrdine, string nuovoStato)
        {
            var ordine = dbContext.Ordini.Find(idOrdine);
            if (ordine != null)
            {
                ordine.Stato = nuovoStato;
                dbContext.SaveChanges();
                TempData["Message"] = "Stato dell'ordine modificato con successo.";
            }
            else
            {
                TempData["ErrorMessage"] = "Ordine non trovato.";
            }

            return RedirectToAction("GestioneOrdini");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Incassi(DateTime? data)
        {
            if (data == null)
            {
                // Se la data non è stata specificata, utilizza la data odierna
                data = DateTime.Today;
            }

            // Calcola il numero di ordini evasi e il totale dell'incasso per la data specificata
            int countOrdiniEvasi = dbContext.Ordini.Where(o => DbFunctions.TruncateTime(o.DataOrdine) == data && o.Stato == "EVASO").Count();
            decimal? totaleIncassoGiornata = dbContext.Ordini.Where(o => DbFunctions.TruncateTime(o.DataOrdine) == data && o.Stato == "EVASO").Sum(o => (decimal?)o.Totale);

            // Imposta i valori nella ViewBag per renderli disponibili nella vista
            ViewBag.CountOrdiniEvasi = countOrdiniEvasi;
            ViewBag.TotaleIncassoGiornata = totaleIncassoGiornata;

            // Imposta la data selezionata nella ViewBag per mantenerla nell'input nella vista
            ViewBag.DataSelezionata = data.Value.ToString("yyyy-MM-dd");

            return View();
        }

    }
}
