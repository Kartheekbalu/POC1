using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookMyMovie.Models;
using BookMyMovie.Models.DbContext;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShowsController : Controller
    {
        private readonly MovieDbContext _context;

        public ShowsController(MovieDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Shows
        public async Task<IActionResult> Index()
        {
            var movieDbContext = _context.Shows.Include(s => s.Movie).Include(s => s.Theater);
            return View(await movieDbContext.ToListAsync());
        }

        // GET: Admin/Shows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .FirstOrDefaultAsync(m => m.ShowId == id);
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }

        // GET: Admin/Shows/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title");
            ViewData["TheaterId"] = new SelectList(_context.Theaters, "TheaterId", "Name");
            return View();
        }

        // POST: Admin/Shows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowId,MovieId,TheaterId,ShowTime")] Show show)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(show);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", show.MovieId);
            ViewData["TheaterId"] = new SelectList(_context.Theaters, "TheaterId", "Name", show.TheaterId);
            return View(show);
        }

        // GET: Admin/Shows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows.FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", show.MovieId);
            ViewData["TheaterId"] = new SelectList(_context.Theaters, "TheaterId", "Name", show.TheaterId);
            return View(show);
        }

        // POST: Admin/Shows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShowId,MovieId,TheaterId,ShowTime")] Show show)
        {
            if (id != show.ShowId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(show);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowExists(show.ShowId))
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
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", show.MovieId);
            ViewData["TheaterId"] = new SelectList(_context.Theaters, "TheaterId", "Name", show.TheaterId);
            return View(show);
        }

        // GET: Admin/Shows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .FirstOrDefaultAsync(m => m.ShowId == id);
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }

        // POST: Admin/Shows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show != null)
            {
                _context.Shows.Remove(show);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowExists(int id)
        {
            return _context.Shows.Any(e => e.ShowId == id);
        }
    }
}
