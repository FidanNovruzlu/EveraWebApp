using EveraWebApp.DataContext;
using EveraWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EveraWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CatagoryController : Controller
    {
        private readonly EveraDbContext _everaDbContext;
        public CatagoryController (EveraDbContext everaDbContext)
        {
            _everaDbContext=everaDbContext;
        }
        public async Task<IActionResult> Index()
        {
            List<Catagory> catagories = await _everaDbContext.Catagories.ToListAsync();
            return View(catagories);
        }
        public async Task<IActionResult> Details(int id)
        {
            Catagory? catagory=await _everaDbContext.Catagories.FindAsync(id);
            if (catagory == null)
            {
                return NotFound();
            }
            return View(catagory);
        }
    }
}
