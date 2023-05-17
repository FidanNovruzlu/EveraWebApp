using System.ComponentModel.DataAnnotations;

namespace EveraWebApp.ViewModels.AccountVM
{
    public class RegisterVM
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; }=null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required,MaxLength(8),DataType(DataType.Password)]
        public string Password { get; set; }= null!;
        [Required,MaxLength(14)]
        public string UserName { get; set; } = null!;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfrimPassword { get; set; } = null!;
    }
}
