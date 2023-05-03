using EveraWebApp.DataContext;
using EveraWebApp.Models;
using EveraWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EveraWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly EveraDbContext _dbContext;

        public HomeController(EveraDbContext everaDbContext)
        {
            _dbContext = everaDbContext;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders= await _dbContext.Sliders.ToListAsync();
            List<Product> products = await _dbContext.Products.Include(c => c.Catagory).ToListAsync();

            HomeVM homeVM = new HomeVM()
            {
                Products=products,
                Sliders=sliders
            };

            return View(homeVM);
        }
      
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}