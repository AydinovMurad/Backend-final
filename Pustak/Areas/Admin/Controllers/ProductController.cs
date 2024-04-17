
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustak.Data;
using Pustak.Extensions;
using Pustak.Models;
using Pustok.Areas.Admin.Dtos;

namespace Pustak.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
                                                   .Include(x => x.ProductImages)
                                                   .Include(x => x.category)
                                                   .ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto dto)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_context.Products.Any(x => x.Name == dto.Name))
            {
                ModelState.AddModelError("", "Product already exists");
                return View(dto);
            }

            if (dto.AdditionalFiles != null)
            {
                foreach (var file in dto.AdditionalFiles)
                {

                    if (!file.CheckFileSize(2))
                    {
                        ModelState.AddModelError("AdditionalFiles", "Files cannot be more than 2mb");
                        return View(dto);
                    }


                    if (!file.CheckFileType("image"))
                    {
                        ModelState.AddModelError("AdditionalFiles", "Files must be image type!");
                        return View(dto);
                    }
                }
            }

            if (!dto.IsMain.CheckFileSize(2))
            {
                ModelState.AddModelError("MainFile", "Files cannot be more than 2mb");
                return View(dto);
            }


            if (!dto.IsMain.CheckFileType("image"))
            {
                ModelState.AddModelError("MainFile", "Files must be image type!");
                return View(dto);
            }

            if (!dto.IsHover.CheckFileSize(2))
            {
                ModelState.AddModelError("HoverFile", "Files cannot be more than 2mb");
                return View(dto);
            }


            if (!dto.IsHover.CheckFileType("image"))
            {
                ModelState.AddModelError("HoverFile", "Files must be image type!");
                return View(dto);
            }


            Product product = new()
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                DiscountPrice = dto.DiscountPrice,
                ExTax = dto.ExTax,
                ProductCode = dto.ProductCode,
                RewardPoint = dto.RewardPoint,
                Availability = dto.Availability,
                Count = dto.Count,
                Rating = dto.Rating,
                CategoryId = dto.CategoryId,
                ProductImages = null
            };

            var mainFileName = await dto.IsMain.SaveFileAsync(_env.WebRootPath, "Client", "image", "products");
            var mainProductImageCreate = CreateProduct(mainFileName, false, true, product);

            List<ProductImage> productImage = new();

            productImage.Add(mainProductImageCreate);

            product.ProductImages = productImage;


            var hoverFileName = await dto.IsHover.SaveFileAsync(_env.WebRootPath, "client", "image", "products");
            var hoverProductImageCreate = CreateProduct(hoverFileName, true, false, product);
            product.ProductImages.Add(hoverProductImageCreate);

            foreach (var file in dto.AdditionalFiles)
            {
                var filename = await file.SaveFileAsync(_env.WebRootPath, "Client", "image", "products");
                var additionalProductImgs = CreateProduct(filename, false, false, product);
                product.ProductImages.Add(additionalProductImgs);
            }

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ProductImage CreateProduct(string url, bool isHover, bool isMain, Product product)
        {
            return new ProductImage
            {
                Url = url,
                IsHover = isHover,
                IsMain = isMain,
                Product = product
            };
        }
    }
}
