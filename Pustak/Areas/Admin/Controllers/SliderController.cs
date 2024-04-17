using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustak.Data;
using Pustak.Extensions;
using Pustak.Models;
using Pustok.Areas.Admin.Dtos;

namespace Pustak.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Slider.ToListAsync();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            if (!dto.File.CheckFileType("image"))
            {
                ModelState.AddModelError("", "Invalid File");
                return View(dto);
            }
            if (!dto.File.CheckFileSize(2))
            {
                ModelState.AddModelError("", "Invalid File Size");
                return View(dto);
            }

            string UniqueFileName = await dto.File.SaveFileAsync(_env.WebRootPath, "Client", "image", "icon");

            Slider newSlider = new()
            {
                Name = dto.Name,
                Description = dto.Description,
                ImagePath = UniqueFileName
            };
            await _context.Slider.AddAsync(newSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var slider = await _context.Slider.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (slider is null)
            {
                return NotFound();
            }

            SliderUpdateDto dto = new()
            {
                Name = slider.Name,
                Description = slider.Description,
                ImagePath = slider.ImagePath
            };


            return View(dto);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, SliderUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var existSlider = await _context.Slider.FirstOrDefaultAsync(x => x.Id == id);

            if (existSlider is null)
                return NotFound();
            if (dto.File is not null)
            {
                if (!dto.File.CheckFileType("image"))
                {
                    ModelState.AddModelError("File", "Invalid File Type");
                    return View(dto);
                }
                if (!dto.File.CheckFileSize(2))
                {
                    ModelState.AddModelError("File", "Invalid File Size");
                    return View(dto);
                }
                existSlider.ImagePath.DeleteFile(_env.WebRootPath, "Client", "image", "products");

                var uniqueFileName = await dto.File.SaveFileAsync(_env.WebRootPath, "Client", "image", "products");
                existSlider.ImagePath = uniqueFileName;
            }
            existSlider.Description = dto.Description;
            existSlider.Name = dto.Name;
            _context.Update(existSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Slider.FirstOrDefaultAsync(x => x.Id == id);

            if (slider is null)
            {
                return NotFound();
            }

            slider.IsDeleted = true;
            slider.ImagePath.DeleteFile(_env.WebRootPath, "Client", "image", "icon");

            _context.Slider.Remove(slider);
            return RedirectToAction("Index");
        }
    }
}
