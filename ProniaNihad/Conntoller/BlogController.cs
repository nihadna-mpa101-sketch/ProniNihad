using Microsoft.AspNetCore.Mvc;

namespace ProniaWebNihad.Conntoller
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
