using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DogDC.Models;
using System.Security.Claims;

namespace DogDC.Controllers
{
    public class CalendarController : Controller
    {
        private readonly DogdcDBContext _context;

        public CalendarController(DogdcDBContext context)
        {
            _context = context;
        }


        //GET: Calendar/Index/Customer
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            //var dogdcDBContext = _context.Bookings.Include(b => b.Utility);
            var dogdcDBContext = _context.Bookings
             .Include(b => b.Utility)
             .Where(p => p.BookingStatuses.FirstOrDefault().CustomerId == userId);

            return View(await dogdcDBContext.ToListAsync());
        }

        // GET: Calendar/Calendar
        public ActionResult Calendar(int? vuosi, int? kuukausi)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.IsAdmin;
            ViewBag.UserIsAdmin = userId;

            ViewData["vuosi"] = vuosi.HasValue ? vuosi.Value : DateTime.Today.Year;
            ViewData["kuukausi"] = kuukausi.HasValue ? kuukausi.Value : DateTime.Today.Month;
            return View();
        }

        // GET: Calendar/Details/Customer
        public async Task<IActionResult> Details(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Utility)
                .Where(p => p.BookingStatuses.FirstOrDefault().CustomerId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Calendar/Create/Customer&Admin
        public IActionResult Create()
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userIsAdminCheck = user?.IsAdmin;
            ViewBag.UserIsAdmin = userIsAdminCheck;

            ViewData["UtilityId"] = new SelectList(_context.Utilities, "Id", "Id");
            //ViewData["UtilityId"] = new SelectList(_context.Utilities, "Name", "Id");
            return View();
        }

        // POST: Calendar/Create/Customer&Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,UtilityId")] Booking booking)
        {

            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
            }
            {
                var bookingId = booking.Id;
                BookingStatus bs = new BookingStatus { CustomerId = userId, BookingId = booking.Id };
                _context.BookingStatuses.Add(bs);
                await _context.SaveChangesAsync();
            }
            ViewData["UtilityId"] = new SelectList(_context.Utilities, "Id", "Id", booking.UtilityId);

            return View(booking);
        }

        // GET: Calendar/Edit/Customer
        public async Task<IActionResult> Edit(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Where(p => p.BookingStatuses.FirstOrDefault().CustomerId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound();
            }
            ViewData["UtilityId"] = new SelectList(_context.Utilities, "Id", "Id", booking.UtilityId);
            return View(booking);
        }

        // POST: Calendar/Edit/Customer&Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,UtilityId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["UtilityId"] = new SelectList(_context.Utilities, "Id", "Id", booking.UtilityId);
            return View(booking);
        }

        // GET: Calendar/Delete/Customer
        public async Task<IActionResult> Delete(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;
         
            var userIsAdminCheck = user?.IsAdmin;
            ViewBag.UserIsAdmin = userIsAdminCheck;

            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
               .Include(p => p.Utility)
               .Where(p => p.BookingStatuses.FirstOrDefault().CustomerId == userId)
               .FirstOrDefaultAsync(p => p.Id == id);

            var bookingstatus = await _context.BookingStatuses
                .Include(p => p.Booking)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(p => p.Booking.Id == id);


            if (booking == null)
            { 
             return NotFound();
            }

            return View(booking);
        }

        // POST: Calendar/Delete/Customer&Admin
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null && _context.BookingStatuses == null)
            {
                return Problem("Entity set 'DogdcDBContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            var bookingstatus = await _context.BookingStatuses
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(p => p.Booking.Id == id);

            if (booking != null && bookingstatus != null)
            {
                _context.BookingStatuses.Remove(bookingstatus);
                _context.Bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
          return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool BookingStatusExists(int id)
        {
            return (_context.BookingStatuses?.Any(e => e.Id == id)).GetValueOrDefault();
        }




        //-------------- TÄSTÄ ALKAA ADMIN OSASTO---------------
        //------------------------------------------------------
        //------------------------------------------------------


        // GET: Calendar/Index/Admin
        [Authorize(Roles = "Administrator")]
        public ActionResult IndexAdmin(string sortOrder)
        {
            var bookings = from b in _context.Bookings
                           .Include(b => b.Utility)
                           select b;
            switch (sortOrder)
            {
                case "group_desc":
                    bookings = bookings.OrderBy(s => s.EndTime);
                    break;
                case "cust_desc":
                    bookings = bookings.OrderBy(s => s.UtilityId);
                    break;
                default:
                    bookings = bookings.OrderBy(s => s.StartTime);
                    break;
            }
            return View("IndexAdmin", bookings.ToList());
        }

        // GET: Calendar/Details/Admin
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DetailsAdmin(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.IsAdmin;
            ViewBag.UserIsAdmin = userId;

            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Utility)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View("Details", booking);
        }

        // GET: Calendar/Edit/Admin
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditAdmin(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userIsAdminCheck = user?.IsAdmin;
            ViewBag.UserIsAdmin = userIsAdminCheck;

            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound();
            }
            ViewData["UtilityId"] = new SelectList(_context.Utilities, "Id", "Id", booking.UtilityId);
            return View("Edit", booking);
        }

        // GET: Calendar/Delete/Admin
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAdmin(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userIsAdminCheck = user?.IsAdmin;
            ViewBag.UserIsAdmin = userIsAdminCheck;

            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
               .Include(p => p.Utility)
               .FirstOrDefaultAsync(p => p.Id == id);

            var bookingstatus = await _context.BookingStatuses
                .Include(p => p.Booking)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(p => p.Booking.Id == id);


            if (booking == null)
            {
                return NotFound();
            }

            return View("Delete", booking);
        }
    }
}
