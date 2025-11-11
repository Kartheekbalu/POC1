using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookMyMovie.Models.DbContext;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class BookingsController : Controller
    {
        private readonly MovieDbContext _context;

        public BookingsController(MovieDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Bookings
        public async Task<IActionResult> Index()
        {
            var movieDbContext = _context.Bookings.Include(b => b.Seat).Include(b => b.Show);
            return View(await movieDbContext.ToListAsync());
        }

        // GET: Admin/Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Seat)
                .Include(b => b.Show)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Admin/Bookings/Create
        public IActionResult Create()
        {
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "SeatNumber");
            ViewData["ShowId"] = new SelectList(_context.Shows, "ShowId", "ShowId");
            return View();
        }

        // POST: Admin/Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,UserId,ShowId,SeatId,BookingTime")] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "SeatNumber", booking.SeatId);
            ViewData["ShowId"] = new SelectList(_context.Shows, "ShowId", "ShowId", booking.ShowId);
            return View(booking);
        }

        // GET: Admin/Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "SeatNumber", booking.SeatId);
            ViewData["ShowId"] = new SelectList(_context.Shows, "ShowId", "ShowId", booking.ShowId);
            return View(booking);
        }

        // POST: Admin/Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,UserId,ShowId,SeatId,BookingTime")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "SeatNumber", booking.SeatId);
            ViewData["ShowId"] = new SelectList(_context.Shows, "ShowId", "ShowId", booking.ShowId);
            return View(booking);
        }

        // GET: Admin/Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Seat)
                .Include(b => b.Show)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Admin/Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
