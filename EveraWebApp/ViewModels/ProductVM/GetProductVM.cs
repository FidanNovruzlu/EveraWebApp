namespace EveraWebApp.ViewModels.ProductVM
{
    public class GetProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set;}
        public string ImageName { get; set; }
    }
}
