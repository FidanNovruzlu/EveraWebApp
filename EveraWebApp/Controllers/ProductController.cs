using EveraWebApp.DataContext;
using EveraWebApp.Models;
using EveraWebApp.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
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
        public async Task< IActionResult> AddCart(int id)
        {
            Product? product=await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            string? value = HttpContext.Request.Cookies["basket"];

            List<CartVM> cartsCookie = new List<CartVM>();
            if (value == null)
            {
                HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(cartsCookie));
            }
            else
            {
                cartsCookie = JsonSerializer.Deserialize < List<CartVM>>(value);
            }
            CartVM? cart= cartsCookie.Find(c=>c.Id==id);
            if (cart == null)
            {
                cartsCookie.Add(new CartVM()
                {
                    Id = id,
                    Count = 1
                }) ;
            }
            else
            {
                cart.Count += 1;
            }
            HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(cartsCookie));
            return RedirectToAction("Index","Home");
        }
        public async Task< IActionResult> GetCarts()
        {
            string? value = HttpContext.Request.Cookies["basket"];
            List<CartVM> cartVM = JsonSerializer.Deserialize<List<CartVM>>(value);
            List<Product> products = new List<Product>();
            foreach (var item in cartVM)
            {
                Product product = await _context.Products.FindAsync(item.Id);
                products.Add(product);
            }
            return View(products);
        }
    }
}
