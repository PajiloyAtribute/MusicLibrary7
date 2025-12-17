using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicLibrary.Models;
using MusicDatabase.Repository;

namespace MusicLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISongRepository _songRepo;

        public HomeController(ISongRepository songRepo)
        {
            _songRepo = songRepo;
        }

        public async Task<IActionResult> Index()
        {
            // show latest 6 songs by id desc
            var all = await _songRepo.GetAllAsync();
            var latest = all.OrderByDescending(s => s.SongId).Take(6).ToList();
            return View(latest);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
