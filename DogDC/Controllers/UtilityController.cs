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
    public class UtilityController : Controller
    {
        private readonly DogdcDBContext _context;
        public UtilityController(DogdcDBContext context)
        {
            _context = context;
        }

        // GET: Utility
        public ActionResult Index(string searchString)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.IsAdmin;
            ViewBag.UserIsAdmin = userId;

            var utilities = from s in _context.Utilities.Include(p => p.Category)
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                utilities = utilities.Where(s => s.Name.Contains(searchString)
                                       || s.Category.Name.Contains(searchString));
            }
            return View(utilities.ToList());
        }
        public async Task<IActionResult> GetUtilitiesByCategory(int id)
        {
            var utilities = _context.Utilities.Include(p => p.Category).Where(i => i.Category.Id == id);
            ViewBag.Header = utilities.First().Category.Name;
            return View("Index", await utilities.ToListAsync());
        }

        // GET: Utility/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Utilities == null)
            {
                return NotFound();
            }

            var utility = await _context.Utilities
                .Include(u => u.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utility == null)
            {
                return NotFound();
            }

            return View(utility);
        }

        // GET: Utility/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: Utility/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,CategoryId")] Utility utility)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", utility.CategoryId);
            return View(utility);
        }

        // GET: Utility/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Utilities == null)
            {
                return NotFound();
            }

            var utility = await _context.Utilities.FindAsync(id);
            if (utility == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", utility.CategoryId);
            return View(utility);
        }

        // POST: Utility/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,CategoryId")] Utility utility)
        {
            if (id != utility.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilityExists(utility.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", utility.CategoryId);
            return View(utility);
        }

        // GET: Utility/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Utilities == null)
            {
                return NotFound();
            }

            var utility = await _context.Utilities
                .Include(u => u.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utility == null)
            {
                return NotFound();
            }

            return View(utility);
        }

        // POST: Utility/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Utilities == null)
            {
                return Problem("Entity set 'DogdcDBContext.Utilities'  is null.");
            }
            var utility = await _context.Utilities.FindAsync(id);
            if (utility != null)
            {
                _context.Utilities.Remove(utility);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilityExists(int id)
        {
          return (_context.Utilities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
