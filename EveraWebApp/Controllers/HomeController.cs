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
            HttpContext.Request.Cookies.Append(new KeyValuePair<string, string>("test", "Hello"));
            //HttpContext.Session.Set("test", new byte[] {1,3,2});
            List<Slider> sliders= await _dbContext.Sliders.ToListAsync();
            List<Popular> populars= await _dbContext.Populars.ToListAsync();
            List<Brand> brands = await _dbContext.Brands.ToListAsync();

            HomeVM homeVM = new HomeVM()
            {
               Sliders=sliders,
               Populars=populars,
               Brands=brands
            };

            return View(homeVM);
        }
        public IActionResult GetSession()
        {
            return Json(HttpContext.Session.Get("test"));
        }
        public IActionResult GetCookie()
        {
            return Json(HttpContext.Request.Cookies["test"]);
        }
        public async Task<IActionResult> ProductDetails()
        {
            List<Image> images =await _dbContext.Images.ToListAsync();
            List<Product> product=await _dbContext.Products.ToListAsync();

            HomeVM homeVM = new HomeVM()
            {
                Images=images,
                Products=product
            };
            return View(homeVM);
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}