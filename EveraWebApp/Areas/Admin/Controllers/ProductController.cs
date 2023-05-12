using EveraWebApp.DataContext;
using EveraWebApp.Models;
using EveraWebApp.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

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
            foreach (Product product in products)
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
            Product? product = await _everaDbContext.Products.Include(p=>p.Catagory).Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)  return NotFound();
            
            ReadProductVM detailProductVM = new ReadProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Images = product.Images.ToList()
            };
            return View(detailProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Product? product = await _everaDbContext.Products.FirstOrDefaultAsync(p=>p.Id==id);
            if (product == null)  return NotFound();
           
            foreach(var item in _everaDbContext.Images.Where(x=>x.ProductId==id).ToList())
            {
                _everaDbContext.Images.Remove(item);
            }
            _everaDbContext.Products.Remove(product);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async  Task<IActionResult> Update(int id)
        {
            Product? product= await _everaDbContext.Products.Include(c=>c.Catagory).Include(i=>i.Images).FirstOrDefaultAsync(p=>p.Id==id);
            UpdateProductVM updateProductVM=new UpdateProductVM()
            {
                Id=product.Id,
                Name=product.Name,
                Description=product.Description,
                Price=product.Price,
                CatagoryId=product.CatagoryId,
               OldImages = product.Images
            };
            List<Catagory> catagories =await _everaDbContext.Catagories.ToListAsync();
            ViewData["catagories"]=catagories;
            return View(updateProductVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateProductVM updateProductVM)
        {
            Product? product= await _everaDbContext.Products.Include(c=>c.Catagory).Include(i=>i.Images).FirstOrDefaultAsync(p=>p.Id==id);
            foreach(var item in product.Images)
            {
                _everaDbContext.Images.Remove(item);
            }
            List<Catagory> catagories= await _everaDbContext.Catagories.ToListAsync();
            if(!ModelState.IsValid)
            {
                ViewData["catagories"] = catagories;
                return View();
            }
            foreach(IFormFile item in updateProductVM.Images)
            {
                string guid=Guid.NewGuid().ToString();
                string newFilename = guid + item.FileName;
            }
            List<Image> images = new List<Image>();
            foreach(IFormFile item in updateProductVM.Images)
            {
                string guid= Guid.NewGuid().ToString();
                string newFilename = guid + item.FileName;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs", "shop", newFilename);
                using(FileStream fileStream=new FileStream(path, FileMode.Create))
                {
                    await item.CopyToAsync(fileStream);
                }
                images.Add(new Image(){
                    ImageName= newFilename
                });
            }
            product.Images= images;
            product.Description = updateProductVM.Description;
            product.Name = updateProductVM.Name;
            product.Price = updateProductVM.Price;
            product.CatagoryId= updateProductVM.CatagoryId;

            _everaDbContext.Products.Update(product);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
