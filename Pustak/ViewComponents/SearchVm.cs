using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustak.Data;
using Pustak.ViewModels;

namespace Pustak.ViewComponents
{
    public class SearchVm : ViewComponent
    {
        readonly private AppDbContext _context;

        public SearchVm(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ProductSearchVm vm = new ProductSearchVm()
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(vm);
        }
    }
}
