using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Pustak.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}