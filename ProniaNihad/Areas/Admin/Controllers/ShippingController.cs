using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaWebNihad.Models;


namespace ProniaWebNihad.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class ShippingController(AppDbContext _context, IWebHostEnvironment _environment) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var shippings = await _context.Shippings.ToListAsync();
            return View(shippings);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ShippingCreateVM svm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!svm.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "File sekil formatinda olmalidir!");
                return View(svm);
            }
            if (svm.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "File olcusu maksimum 2MB ola biler!");
                return View(svm);
            }
            string ImageFileName = Guid.NewGuid().ToString() + svm.Image.FileName;
            string ImagePath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images", ImageFileName);
            using FileStream Stream = new(ImagePath, FileMode.Create);
            await svm.Image.CopyToAsync(Stream);
            Shipping shipping = new()
            {
                Name = svm.Name,
                Description = svm.Description,
                ImagePath = ImageFileName

            };
            await _context.Shippings.AddAsync(shipping);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);
            if (shipping is null) return NotFound();
            _context.Shippings.Remove(shipping);
            await _context.SaveChangesAsync();
            string folderUrl = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            string ImagePath = Path.Combine(folderUrl, shipping.ImagePath);

            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);
            if (shipping is not { }) return NotFound();
            return View(shipping);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Shipping shipping)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existShipping = await _context.Shippings.FindAsync(shipping.Id);
            if (existShipping is null) return NotFound();
            existShipping.Name = shipping.Name;
            existShipping.Description = shipping.Description;
            existShipping.ImagePath = shipping.ImagePath;
            _context.Shippings.Update(existShipping);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Info(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);
            if (shipping is null) return NotFound();
            return View(shipping);
        }
    }
}