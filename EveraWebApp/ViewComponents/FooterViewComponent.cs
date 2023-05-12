using EveraWebApp.DataContext;
using EveraWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EveraWebApp.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly EveraDbContext _everaDbContext;
        public FooterViewComponent(EveraDbContext everaDbContext)
        {
            _everaDbContext = everaDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, Setting> setting = await _everaDbContext.Settings.ToDictionaryAsync(s => s.Key);
            return View(setting);
        }
    }
}
