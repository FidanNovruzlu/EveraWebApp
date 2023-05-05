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
        public SliderController(EveraDbContext everaDbContext)
        {
            _everaDbContext = everaDbContext;
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
            if (!ModelState.IsValid) return View();

            Slider? slider = await _everaDbContext.Sliders.FindAsync(id);
            if (slider == null) return NotFound();

            if (_everaDbContext.Sliders.Any(s => s.Title.Trim().ToLower() == newSlider.Title.Trim().ToLower()))
            {
                ModelState.AddModelError("Title", "Already title exsit!");
                return View();
            }
            slider.Title = newSlider.Title;
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Slider? slider = await _everaDbContext.Sliders.FindAsync(id);
            if (slider == null) return NotFound();

            _everaDbContext.Sliders.Remove(slider);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
