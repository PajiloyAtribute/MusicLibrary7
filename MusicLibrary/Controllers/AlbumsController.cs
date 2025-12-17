using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicDatabase.Models;
using MusicDatabase.Repository;

namespace MusicLibrary.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumRepository _repo;
        private readonly IArtistRepository _artistRepo;

        public AlbumsController(IAlbumRepository repo, IArtistRepository artistRepo)
        {
            _repo = repo;
            _artistRepo = artistRepo;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _repo.GetAllAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var album = await _repo.GetByIdAsync(id);
            if (album == null) return NotFound();
            return View(album);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Artists = await _artistRepo.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Album album)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Artists = await _artistRepo.GetAllAsync();
                return View(album);
            }

            await _repo.AddAsync(album);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var album = await _repo.GetByIdAsync(id);
            if (album == null) return NotFound();
            ViewBag.Artists = await _artistRepo.GetAllAsync();
            return View(album);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Album album)
        {
            if (id != album.AlbumId) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Artists = await _artistRepo.GetAllAsync();
                return View(album);
            }

            await _repo.UpdateAsync(album);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var album = await _repo.GetByIdAsync(id);
            if (album == null) return NotFound();
            return View(album);
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
