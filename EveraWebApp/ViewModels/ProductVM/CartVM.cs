using EveraWebApp.Models;

namespace EveraWebApp.ViewModels.ProductVM
{
    public class CartVM
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
    }
}
