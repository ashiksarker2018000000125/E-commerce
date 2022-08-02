using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyyApp.DataAccessLayer.Data;

namespace MyAppWeb.Controllers
{
    public class ImageUploadsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageUploadsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: ImageUploads
        public async Task<IActionResult> Index()
        {
              return _context.ImageUploads != null ? 
                          View(await _context.ImageUploads.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ImageUploads'  is null.");
        }

        // GET: ImageUploads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ImageUploads == null)
            {
                return NotFound();
            }

            var imageUpload = await _context.ImageUploads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageUpload == null)
            {
                return NotFound();
            }

            return View(imageUpload);
        }

        // GET: ImageUploads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImageUploads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ImageFile")] ImageUpload imageUpload)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageUpload.ImageFile.FileName);
                string extension = Path.GetExtension(imageUpload.ImageFile.FileName);
                imageUpload.Title =fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/",fileName);

                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await imageUpload.ImageFile.CopyToAsync(filestream);
                }

                _context.Add(imageUpload);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageUpload);
        }

        // GET: ImageUploads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ImageUploads == null)
            {
                return NotFound();
            }

            var imageUpload = await _context.ImageUploads.FindAsync(id);
            if (imageUpload == null)
            {
                return NotFound();
            }
            return View(imageUpload);
        }

        // POST: ImageUploads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] ImageUpload imageUpload)
        {
            if (id != imageUpload.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageUpload);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageUploadExists(imageUpload.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(imageUpload);
        }

        // GET: ImageUploads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ImageUploads == null)
            {
                return NotFound();
            }

            var imageUpload = await _context.ImageUploads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageUpload == null)
            {
                return NotFound();
            }

            return View(imageUpload);
        }

        // POST: ImageUploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ImageUploads == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ImageUploads'  is null.");
            }
            var imageUpload = await _context.ImageUploads.FindAsync(id);
            if (imageUpload != null)
            {
                _context.ImageUploads.Remove(imageUpload);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageUploadExists(int id)
        {
          return (_context.ImageUploads?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
