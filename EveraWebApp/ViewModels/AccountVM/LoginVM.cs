using System.ComponentModel.DataAnnotations;

namespace EveraWebApp.ViewModels.AccountVM
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
