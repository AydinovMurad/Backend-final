using Microsoft.AspNetCore.Mvc;
using Pustak.Data;

namespace Pustak.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext  _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
