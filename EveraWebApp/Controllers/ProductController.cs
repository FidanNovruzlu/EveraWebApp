using EveraWebApp.DataContext;
using EveraWebApp.Models;
using EveraWebApp.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

namespace EveraWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly EveraDbContext _context;
        public ProductController(EveraDbContext everaDbContext)
        {
            _context= everaDbContext;
        }
        
       
        public IActionResult AddCart(int id)
        {
            Product? product = _context.Products.Include(x=>x.Catagory).Include(x=>x.Images).FirstOrDefault(x=> x.Id==id);
            if (product == null) return NotFound();

            string? value = HttpContext.Request.Cookies["basket"];

            List<CartVM> cartVms = new List<CartVM>();
            if (value == null)
            {
                HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(cartVms));
            }
            else
            {
                cartVms = JsonSerializer.Deserialize<List<CartVM>>(value);
            }
            CartVM? oldCart = cartVms.FirstOrDefault(c => c.Id == id);
            if (oldCart == null)
            {
                cartVms.Add(new CartVM()
                {
                    Id = id,
                    Count = 1,
                    Name=product.Name,
                    Price  =(double)product.Price,
                    ImageUrl=product.Images.FirstOrDefault().ImageName,
                    CategoryName=product.Catagory.Name
                });
            }
            else
            {
                oldCart.Count += 1;
            }
            HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(cartVms),new CookieOptions{
                MaxAge=TimeSpan.FromMinutes(10)
            });
            return RedirectToAction("Index","Home");
        }
        public  IActionResult GetCarts()
        {
            string value = HttpContext.Request.Cookies["basket"];
            List<CartVM> cartVM = JsonSerializer.Deserialize<List<CartVM>>(value);
           
            return View(cartVM);
        }
    }
}
