using BookMyMovie.Models;
using BookMyMovie.Models.DbContext;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMyMovie.Areas.Admin.Controllers
{

    [Authorize]
    [Authorize(policy: "Admin")]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly MovieDbContext _context;

        public DashboardController(MovieDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalMovies = _context.Movies.Count();
            var totalTheaters = _context.Theaters.Count();
            var totalShows = _context.Shows.Count();
            var totalBookings = _context.Bookings.Count();

            ViewBag.TotalMovies = totalMovies;
            ViewBag.TotalTheaters = totalTheaters;
            ViewBag.TotalShows = totalShows;
            ViewBag.TotalBookings = totalBookings;

            return View();
        }
    }
}
