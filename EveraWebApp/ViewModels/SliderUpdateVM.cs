using System.ComponentModel.DataAnnotations.Schema;

namespace EveraWebApp.ViewModels
{
    public class SliderUpdateVM
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
