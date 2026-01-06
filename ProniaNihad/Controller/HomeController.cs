using Microsoft.AspNetCore.Mvc;
using ProniaWebNihad;

namespace ProniaWebNihad.Conntoller
{
    public class HomeController : Controller
    {
        private readonly AddDbContext _context;
        public HomeController(AddDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var shippings = _context.Shippings.ToList();
            return View(shippings);
        }

    }
}
