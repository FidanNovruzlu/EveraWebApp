namespace EveraWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } = 0;
        public int CatagoryId { get; set;}
        public Catagory Catagory { get; set; }
        public List<Color> Colors { get; set;}
    }
}
