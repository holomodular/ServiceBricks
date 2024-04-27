using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModel.Home
{
    public class RegisterAdminViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}