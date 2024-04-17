using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustak.Data;
using Pustak.Extensions;
using Pustak.Models;

namespace Pustak.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Include(x => x.Products).Where(x => !x.IsDeleted).ToListAsync();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {  Category newCategory = new Category
            {
                Name = category.Name
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return View("404");
            }
            Category? category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return View("404");
            }
            return View(category);
        }


        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id) return BadRequest();
            Category? existsCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (existsCategory == null) return NotFound();
            else
            {
                _context.Categories.Update(category);

            }
            await _context.SaveChangesAsync();
            if (category.Name == null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            category.IsDeleted = true; 
            await _context.SaveChangesAsync();

            var categories = await _context.Categories.Include(x => x.Products).Where(x => !x.IsDeleted).ToListAsync();

            return PartialView("_CategoryPartial", categories);
        }
    }
}
