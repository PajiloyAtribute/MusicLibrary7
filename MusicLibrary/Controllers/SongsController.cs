using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicDatabase.Models;
using MusicDatabase.Repository;

namespace MusicLibrary.Controllers
{
    public class SongsController : Controller
    {
        private readonly ISongRepository _repo;
        private readonly IAlbumRepository _albumRepo;

        public SongsController(ISongRepository repo, IAlbumRepository albumRepo)
        {
            _repo = repo;
            _albumRepo = albumRepo;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _repo.GetAllAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var song = await _repo.GetSongWithArtistsAsync(id);
            if (song == null) return NotFound();
            return View(song);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int? albumId)
        {
            ViewBag.Albums = await _albumRepo.GetAllAsync();
            ViewBag.SelectedAlbumId = albumId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Song song, int? albumId)
        {
            if (albumId.HasValue)
            {
                song.AlbumId = albumId.Value;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Albums = await _albumRepo.GetAllAsync();
                ViewBag.SelectedAlbumId = albumId;
                return View(song);
            }

            await _repo.AddAsync(song);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var song = await _repo.GetByIdAsync(id);
            if (song == null) return NotFound();
            ViewBag.Albums = await _albumRepo.GetAllAsync();
            return View(song);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Song song)
        {
            if (id != song.SongId) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Albums = await _albumRepo.GetAllAsync();
                return View(song);
            }

            await _repo.UpdateAsync(song);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await _repo.GetByIdAsync(id);
            if (song == null) return NotFound();
            return View(song);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddArtist(int id)
        {
            var song = await _repo.GetByIdAsync(id);
            if (song == null) return NotFound();
            ViewBag.Artists = await _albumRepo.GetAlbumsByArtistIdAsync(0); // placeholder
            return View(song);
        }
    }
}
