using EveraWebApp.DataContext;
using EveraWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Runtime.Intrinsics.X86;

namespace EveraWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PopularController : Controller
    {
        private readonly EveraDbContext _everaDbContext;
        private readonly IWebHostEnvironment _environment;
        public PopularController(EveraDbContext everaDbContext, IWebHostEnvironment environment)
        {
            _everaDbContext = everaDbContext;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            List<Popular> populars =  await _everaDbContext.Populars.ToListAsync();
            return View(populars);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Popular? popular=await _everaDbContext.Populars.FindAsync(id);
            if (popular == null) return NotFound();
            if(popular.ImageName!=null)
            {
                string filePath = Path.Combine(_environment.WebRootPath, "assets", "imgs", "shop");
                if(System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _everaDbContext.Populars.Remove(popular);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Popular popular)
        {
            if(!ModelState.IsValid) return View();
            if(_everaDbContext.Populars.Any(p=>p.Title.Trim().ToLower()== popular.Title.Trim().ToLower()))
            {
                ModelState.AddModelError("Title", "Already title exist!");
                return View();
            }

            string guid=Guid.NewGuid().ToString();
            string newFilename = guid + popular.Image.FileName;
            string path = Path.Combine(_environment.WebRootPath, "assets", "imgs", "shop", newFilename);

            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
            {
                await popular.Image.CopyToAsync(fileStream);
            }

            popular.ImageName = newFilename;
            await _everaDbContext.Populars.AddAsync(popular);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Read(int id)
        {
            Popular? popular = await _everaDbContext.Populars.FindAsync(id);
            if (popular == null) return NotFound();
            return View(popular);
        }
    }
}
