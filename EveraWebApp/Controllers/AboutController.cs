using EveraWebApp.DataContext;
using EveraWebApp.Models;
using EveraWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EveraWebApp.Controllers
{
    public class AboutController : Controller
    {
        private readonly EveraDbContext _everaDbContext;
        public AboutController(EveraDbContext everaDbContext)
        {
            _everaDbContext = everaDbContext;
        }

        public async Task< IActionResult> Index()
        {
          
            return View();
        }
    }
}
