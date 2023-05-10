using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EveraWebApp.ViewModels.SliderVM
{
    public class UpdateSliderVM
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; } 
    }
}
