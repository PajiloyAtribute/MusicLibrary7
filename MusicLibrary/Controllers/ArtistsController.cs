using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicDatabase.Models;
using MusicDatabase.Repository;

namespace MusicLibrary.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistRepository _repo;
        private readonly IWebHostEnvironment _env;

        public ArtistsController(IArtistRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            const int PageSize = 10;
            var items = await _repo.GetPagedAsync(page, PageSize, search);
            ViewBag.Search = search;
            ViewBag.Page = page;
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var artist = await _repo.GetArtistWithSongsAsync(id);
            if (artist == null) return NotFound();
            return View(artist);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Artist artist, IFormFile? photo)
        {
            if (!ModelState.IsValid) return View(artist);

            if (photo != null && photo.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images", "artists");
                Directory.CreateDirectory(uploads);
                var fileName = Path.GetFileName(photo.FileName);
                var full = Path.Combine(uploads, fileName);
                using (var fs = new FileStream(full, FileMode.Create))
                {
                    await photo.CopyToAsync(fs);
                }
                artist.PhotoFileName = fileName;
            }

            await _repo.AddAsync(artist);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var artist = await _repo.GetByIdAsync(id);
            if (artist == null) return NotFound();
            return View(artist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Artist artist, IFormFile? photo)
        {
            if (id != artist.ArtistId) return BadRequest();
            if (!ModelState.IsValid) return View(artist);

            if (photo != null && photo.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images", "artists");
                Directory.CreateDirectory(uploads);
                var fileName = Path.GetFileName(photo.FileName);
                var full = Path.Combine(uploads, fileName);
                using (var fs = new FileStream(full, FileMode.Create))
                {
                    await photo.CopyToAsync(fs);
                }
                artist.PhotoFileName = fileName;
            }

            await _repo.UpdateAsync(artist);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var artist = await _repo.GetByIdAsync(id);
            if (artist == null) return NotFound();
            return View(artist);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
