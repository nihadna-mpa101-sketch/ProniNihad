using ProniaWebNihad.Helpers;
using System.Runtime.Serialization;

namespace ProniaWebSeyid.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class ProductController(AppDbContext _context, IWebHostEnvironment _environment) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(x => x.Category).Select(product => new ProductGetVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryName = product.Category.Name,
                Price = product.Price,
                RatingCount = product.RatingCount,
                MainImagePath = product.MainImagePath,
                HoverImagePath = product.HoverImagePath

            }).ToListAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await ViewsBagItem();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {

            if (!ModelState.IsValid)
            {
                await ViewsBagItem();
                return View(vm);
            }
            var isExistingCategory = await _context.Categories.AnyAsync(c => c.Id == vm.CategoryId);
            if (!isExistingCategory)
            {
                await ViewsBagItem();
                ModelState.AddModelError("CategoryId", "Secdiyiniz Kateqoriya yoxdu!");
                return View(vm);
            }
            foreach (var tagId in vm.TagIds)
            {
                var isExistTag = await _context.Tags.AnyAsync(x => x.Id == tagId);
                await ViewsBagItem();
                if (!isExistTag)
                {

                    ModelState.AddModelError("TagIds", "Bele bir tag yoxdur");
                    return View(vm);
                }
            }
            if (vm.RatingCount > 6 || vm.RatingCount < 0)
            {
                ModelState.AddModelError("RatingCount", "Ratign0-5 arasi olmalidi!");
                return View(vm);
            }

            if (!vm.MainImage.CheckType())
            {
                ModelState.AddModelError("MainImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.MainImage.CheckSize(2))
            {
                ModelState.AddModelError("MainImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            if (!vm.HoverImage.CheckType())
            {
                ModelState.AddModelError("HoverImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.HoverImage.CheckSize(2))
            {
                ModelState.AddModelError("HoverImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");

            foreach (var image in vm.Images)
            {
                if (!image.CheckType())
                {
                    ModelState.AddModelError("Images", "File sekil formatinda olmalidir!");
                    return View(vm);
                }
                if (image.CheckSize(2))
                {
                    ModelState.AddModelError("Images", "File olcusu maksimum 2MB ola biler!");
                    return View(vm);
                }
            }
            string mainImageFileName = await vm.MainImage.SaveFileAsync(folderPath);
            string hoverImageFileName = await vm.HoverImage.SaveFileAsync(folderPath);

            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                MainImagePath = mainImageFileName,
                HoverImagePath = hoverImageFileName,
                RatingCount = vm.RatingCount,
                ProductTags = [],
                ProductImages = []
            };
            foreach (var image in vm.Images)
            {
                string ImagesFileName = await image.SaveFileAsync(folderPath);
                ProductImage productImage = new()
                {
                    ImagePath = ImagesFileName,
                    Product = product
                };
                product.ProductImages.Add(productImage);
            }
            foreach (var tagId in vm.TagIds)
            {
                ProductTag productTag = new()
                {
                    TagId = tagId,
                    Product = product
                };
                product.ProductTags.Add(productTag);

            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.Include(x => x.ProductTags).Include(i => i.ProductImages).SingleOrDefaultAsync(x => x.Id == id);
            if (product is null) return NotFound();
            await ViewsBagItem();
            ProductUpdateVM vm = new ProductUpdateVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                MainImagePath = product.MainImagePath,
                HoverImagePath = product.HoverImagePath,
                TagIds = product.ProductTags.Select(x => x.TagId).ToList(),
                ImagePath = product.ProductImages.Select(x => x.ImagePath).ToList(),
                ImagesPathIds = product.ProductImages.Select(x => x.Id).ToList()
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await ViewsBagItem();
                return View(vm);
            }

            var existProduct = await _context.Products.Include(x => x.ProductTags).Include(i => i.ProductImages).FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (existProduct is null) return NotFound();
            var isExistingCategory = await _context.Categories.AnyAsync(c => c.Id == vm.CategoryId);
            if (!isExistingCategory)
            {
                ModelState.AddModelError("CategoryId", "Secdiyiniz Kateqoriya yoxdu!");
                return View(vm);
            }
            foreach (var tagId in vm.TagIds)
            {
                var isExistTag = await _context.Tags.AnyAsync(x => x.Id == tagId);
                await ViewsBagItem();
                if (!isExistTag)
                {

                    ModelState.AddModelError("TagIds", "Bele bir tag yoxdur");
                    return View(vm);
                }
            }
            if (vm.RatingCount > 6 || vm.RatingCount < 0)
            {
                ModelState.AddModelError("RatingCount", "Ratign0-5 arasi olmalidi!");
                return View(vm);
            }

            if (!vm.MainImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("MainImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.MainImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("MainImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            if (!vm.HoverImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("HoverImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.HoverImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("HoverImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            existProduct.Name = vm.Name;
            existProduct.Description = vm.Description;
            existProduct.CategoryId = vm.CategoryId;
            existProduct.Price = vm.Price;
            existProduct.ProductTags = [];
            foreach (var tagId in vm.TagIds)
            {
                ProductTag productTag = new()
                {
                    TagId = tagId,
                    ProductId = existProduct.Id
                };
                existProduct.ProductTags.Add(productTag);
            }


            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            if (vm.MainImage is { })
            {
                string newMainImage = await vm.MainImage.SaveFileAsync(folderPath);
                string existMainImage = Path.Combine(folderPath, existProduct.MainImagePath);

                ExtensionMethods.DeleteFile(existMainImage);
                existProduct.MainImagePath = newMainImage;
            }
            if (vm.HoverImage is { })
            {
                string newHoverImage = await vm.HoverImage.SaveFileAsync(folderPath);
                string existHoverImage = Path.Combine(folderPath, existProduct.HoverImagePath);

                ExtensionMethods.DeleteFile(existHoverImage);
                existProduct.HoverImagePath = newHoverImage;
            }
            var existImages = existProduct.ProductImages.ToList();
            foreach (var image in existImages)
            {
                var existImageId = vm.ImagesPathIds?.Any(x => x == image.Id) ?? false;
                if (!existImageId)
                {
                    string deletableImagePath = Path.Combine(folderPath, image.ImagePath);
                    ExtensionMethods.DeleteFile(deletableImagePath);
                    existProduct.ProductImages.Remove(image);
                }
            }
            foreach (var image in vm.Images ?? [])
            {
                string ImageFilePath = await image.SaveFileAsync(folderPath);
                ProductImage productImage = new()
                {
                    ImagePath = ImageFilePath,
                    ProductId = existProduct.Id,
                };
                existProduct.ProductImages.Add(productImage);
            }

            _context.Products.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync();
            if (product is null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            string hoverImagePath = Path.Combine(folderPath, product.HoverImagePath);
            string mainImagePath = Path.Combine(folderPath, product.MainImagePath);

            ExtensionMethods.DeleteFile(hoverImagePath);
            ExtensionMethods.DeleteFile(mainImagePath);
            foreach (var image in product.ProductImages)
            {
                string imagePath = Path.Combine(folderPath, image.ImagePath);
                ExtensionMethods.DeleteFile(imagePath);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Info(int id)
        {
            var product = await _context.Products.Include(x => x.Category).Select(product => new ProductGetVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryName = product.Category.Name,
                Price = product.Price,
                RatingCount = product.RatingCount,
                MainImagePath = product.MainImagePath,
                HoverImagePath = product.HoverImagePath,
                TagNames = product.ProductTags.Select(x => x.Tag.Name).ToList(),
                ImagePath = product.ProductImages.Select(x => x.ImagePath).ToList()

            }).FirstOrDefaultAsync(x => x.Id == id);
            if (product is null) return NotFound();
            return View(product);
        }
        private async Task ViewsBagItem()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            var tags = await _context.Tags.ToListAsync();
            ViewBag.Tags = tags;
        }
    }
}