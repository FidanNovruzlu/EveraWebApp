using EveraWebApp.DataContext;
using EveraWebApp.Models;
using EveraWebApp.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EveraWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        public readonly EveraDbContext _everaDbContext;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(EveraDbContext everaDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _everaDbContext = everaDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _everaDbContext.Products.Include(p=>p.Images).ToListAsync();
            List<GetProductVM> getProductVMs = new List<GetProductVM>();
            foreach (var product in products)
            {
                
                getProductVMs.Add(new GetProductVM()
                {
                    Name = product.Name,
                    Price = product.Price,
                    Id = product.Id,
                    ImageName=product.Images.FirstOrDefault().ImageName
                   
                });
            }
            return View(getProductVMs);
        }
        public async Task<IActionResult> Create()
        {
            List<Catagory> catagories = await _everaDbContext.Catagories.ToListAsync();
            ViewData["catagories"]= catagories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductVM newProduct)
        {
            List<Catagory> catagories = await _everaDbContext.Catagories.ToListAsync();
            if (!ModelState.IsValid)
            {
                ViewData["catagories"] = catagories;
                return View();
            }
            Product product=new Product()
            {
                Name = newProduct.Name,
                Price = newProduct.Price,
                CatagoryId= newProduct.CatagoryId,
                Description=newProduct.Description
            };
            List<Image> images = new List<Image>();
            foreach(IFormFile item in newProduct.Images)
            {
                string guid= Guid.NewGuid().ToString();
                string newFilename = guid + item.FileName;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs", "shop", newFilename);
                using(FileStream fileStream=new FileStream(path, FileMode.CreateNew))
                {
                    await item.CopyToAsync(fileStream);
                }
                images.Add(new Image()
                {
                    ImageName= newFilename
                });
            }
            product.Images = images;
            await _everaDbContext.Products.AddAsync(product);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Read(int id)
        {
            Product? product = await _everaDbContext.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ReadProductVM detailProductVM = new ReadProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageName= product.Images.FirstOrDefault().ImageName
            };
            return View(detailProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _everaDbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _everaDbContext.Products.Remove(product);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
