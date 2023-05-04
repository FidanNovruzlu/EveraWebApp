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
        public IActionResult Create()
        {
            return View();  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(Catagory catagory)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if (_everaDbContext.Catagories.Any(c => c.Name.Trim().ToLower() == catagory.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Catagery Already exist");
                return View();
            }
            await _everaDbContext.Catagories.AddAsync(catagory);
            await _everaDbContext.SaveChangesAsync();
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Catagory? catagory = await _everaDbContext.Catagories.FindAsync(id);
            if(catagory == null)
            {
                return NotFound();
            }
            _everaDbContext.Catagories.Remove(catagory);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
