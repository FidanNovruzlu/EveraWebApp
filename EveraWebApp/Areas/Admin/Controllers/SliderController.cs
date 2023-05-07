using EveraWebApp.DataContext;
using EveraWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing.Text;

namespace EveraWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly EveraDbContext _everaDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(EveraDbContext everaDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _everaDbContext = everaDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _everaDbContext.Sliders.ToListAsync();
            return View(sliders);
        }
      
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if(!ModelState.IsValid) return View();

            if(_everaDbContext.Sliders.Any(s=>s.Title.Trim().ToLower() == slider.Title.Trim().ToLower()))
            {
                ModelState.AddModelError("Title", "Already title exsit!");
                return View();
            }
            string guid = Guid.NewGuid().ToString();
            string newFilename = guid + slider.Image.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs", "slider",newFilename);
         
            using (FileStream fileStream =new FileStream(path, FileMode.CreateNew))
            {
                await slider.Image.CopyToAsync(fileStream);
            }

            slider.ImageName=newFilename;

            await _everaDbContext.Sliders.AddAsync(slider);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       
        public async Task<IActionResult> Read(int id)
        {
            Slider? slider = await _everaDbContext.Sliders.FindAsync(id);
            if(slider== null) return NotFound();
            return View(slider);
        }
     
        public async Task<IActionResult> Update(int id)
        {
            Slider? slider = await _everaDbContext.Sliders.FindAsync(id);
            if(slider==null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Slider newSlider)
        {
            Slider? slider = await _everaDbContext.Sliders.AsNoTracking().Where(s=>s.Id==id).FirstOrDefaultAsync();
            if (slider == null) return NotFound();
            if (!ModelState.IsValid)
            {
                newSlider.ImageName=slider.ImageName;
                return View(newSlider);
            }

            if (_everaDbContext.Sliders.Any(s => s.Title.Trim().ToLower() == newSlider.Title.Trim().ToLower()))
            {
                ModelState.AddModelError("Title", "Already title exsit!");
                return View();
            }
            newSlider.ImageName=slider.ImageName;
        
            _everaDbContext.Sliders.Update(newSlider);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Slider? slider = await _everaDbContext.Sliders.FindAsync(id);
            if (slider == null) return NotFound();

            if (slider.ImageName != null)
            {
                string filePath=Path.Combine(_webHostEnvironment.WebRootPath,"assets","imgs","slider");
                if(System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _everaDbContext.Sliders.Remove(slider);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
