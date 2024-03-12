using System.ComponentModel.DataAnnotations;

namespace back_end_s7.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Inserisci l'Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Inserisci la Password")]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
    }
}
