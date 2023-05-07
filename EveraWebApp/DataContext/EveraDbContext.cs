using EveraWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EveraWebApp.DataContext
{
    public class EveraDbContext : DbContext
    {
        public EveraDbContext(DbContextOptions<EveraDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Catagory > Catagories { get; set; }
        public DbSet<Popular> Populars { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}
