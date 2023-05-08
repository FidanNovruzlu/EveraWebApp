using System.ComponentModel.DataAnnotations.Schema;

namespace EveraWebApp.Models
{
    public class Popular
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageName { get; set; }
       
        [NotMapped]
        public IFormFile Image { get; set; } = null!;
    }
}
