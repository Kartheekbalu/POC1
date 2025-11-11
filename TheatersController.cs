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
    public class TheatersController : Controller
    {
        private readonly MovieDbContext _context;

        public TheatersController(MovieDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Theaters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Theaters.ToListAsync());
        }

        // GET: Admin/Theaters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theater = await _context.Theaters
                .FirstOrDefaultAsync(m => m.TheaterId == id);
            if (theater == null)
            {
                return NotFound();
            }

            return View(theater);
        }

        // GET: Admin/Theaters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Theaters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TheaterId,Name,Location")] Theater theater)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(theater);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theater);
        }

        // GET: Admin/Theaters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null)
            {
                return NotFound();
            }
            return View(theater);
        }

        // POST: Admin/Theaters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TheaterId,Name,Location")] Theater theater)
        {
            if (id != theater.TheaterId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(theater);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheaterExists(theater.TheaterId))
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
            return View(theater);
        }

        // GET: Admin/Theaters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theater = await _context.Theaters
                .FirstOrDefaultAsync(m => m.TheaterId == id);
            if (theater == null)
            {
                return NotFound();
            }

            return View(theater);
        }

        // POST: Admin/Theaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);
            if (theater != null)
            {
                _context.Theaters.Remove(theater);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TheaterExists(int id)
        {
            return _context.Theaters.Any(e => e.TheaterId == id);
        }
    }
}
