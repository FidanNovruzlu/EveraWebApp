using EveraWebApp.DataContext;
using EveraWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EveraWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly EveraDbContext _everaDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BrandController(EveraDbContext everaDbContext,IWebHostEnvironment webHostEnvironment)
        {
            _everaDbContext = everaDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Brand> brands = await _everaDbContext.Brands.ToListAsync();
            return View(brands);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Brand? brand=await _everaDbContext.Brands.FindAsync(id);
            if(brand == null) return NotFound();

            if(brand.ImageName!=null)
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs", "banner");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _everaDbContext.Brands.Remove(brand);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid) return View();
            
            string guid=Guid.NewGuid().ToString();
            string newFilename = guid + brand.Image.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs", "banner", newFilename);

            using(FileStream fileStream=new FileStream(path, FileMode.CreateNew))
            {
                await brand.Image.CopyToAsync(fileStream);
            }
            brand.ImageName = newFilename;
            await _everaDbContext.Brands.AddAsync(brand);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Read(int id)
        {
            Brand? brand = await _everaDbContext.Brands.FindAsync(id);
            if (brand == null) return NotFound();
            return View(brand);
        }
        public async Task<IActionResult> Update(int id)
        {
            Brand? brand = await _everaDbContext.Brands.FindAsync(id);
            if (brand == null) return NotFound();
            return View(brand);
        }
    }
}
