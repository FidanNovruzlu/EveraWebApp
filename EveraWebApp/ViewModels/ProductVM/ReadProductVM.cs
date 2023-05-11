

using EveraWebApp.Models;

namespace EveraWebApp.ViewModels.ProductVM
{
    public class ReadProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CatagoryId { get; set; }
        public string ImageName { get; set; }
        public List<Image> Images { get; set; }
    }
}
