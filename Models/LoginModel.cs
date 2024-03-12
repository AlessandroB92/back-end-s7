using System.ComponentModel.DataAnnotations;

namespace back_end_s7.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
    }
}
