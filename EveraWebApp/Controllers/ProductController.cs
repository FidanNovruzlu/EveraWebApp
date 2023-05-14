using EveraWebApp.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EveraWebApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult AddCart(int id)
        {
            CartVM cartVM = new CartVM()
            {
                Id = id,
                Count = 1
            };
            HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(cartVM));
            return Json("Ok");
        }
        public IActionResult GetCarts()
        {
            string value = HttpContext.Request.Cookies["basket"];
            return Json(value);
        }
    }
}
