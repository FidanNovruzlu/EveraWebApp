using System.ComponentModel.DataAnnotations.Schema;

namespace EveraWebApp.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; } = null!;
    }
}
