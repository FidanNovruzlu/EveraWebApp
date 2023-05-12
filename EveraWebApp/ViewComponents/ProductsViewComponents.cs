using EveraWebApp.DataContext;
using EveraWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EveraWebApp.ViewComponents
{
    public class ProductsViewComponents : ViewComponent
    {
        private readonly EveraDbContext _context;
        public ProductsViewComponents(EveraDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
           List<Product> products =await _context.Products.Include(p=>p.Images).ToListAsync();
            return View(products);
        }
    }
}
