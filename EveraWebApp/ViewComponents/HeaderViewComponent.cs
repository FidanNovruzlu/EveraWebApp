using EveraWebApp.DataContext;
using EveraWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;

namespace EveraWebApp.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly EveraDbContext _everaDbContext;
        public HeaderViewComponent(EveraDbContext everaDbContext)
        {
            _everaDbContext = everaDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string,Setting> settings=await _everaDbContext.Settings.ToDictionaryAsync(s=>s.Key);
            return View(settings);
        }
    }
}
