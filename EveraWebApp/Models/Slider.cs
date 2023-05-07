using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EveraWebApp.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }=null!;
    }
}
