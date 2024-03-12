using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace back_end_s7.Models
{
    public class LoginUtenti
    {
        [Required(ErrorMessage = "Il campo Username è obbligatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Il campo Password è obbligatorio.")]
        public string Password { get; set; }
        public string Ruolo { get; set; }
    }
}