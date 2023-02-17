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
    public class PetController : Controller
    {
        private readonly DogdcDBContext _context;

        public PetController(DogdcDBContext context)
        {
            _context = context;
        }

        //GET: Pet/Index/Customer
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            var userId = user?.IsAdmin;
            ViewBag.UserIsAdmin = userId;

            var dogdcDBContext = _context.Pets
                .Include(p => p.Customer)
                .Include(p => p.Group)
                .Where(p => p.Customer.Email == userEmail)
                ;
            return View(await dogdcDBContext.ToListAsync());
        }

        // GET: Pet/Details/Customer
        public async Task<IActionResult> Details(int? id)
        {

            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            if (id == null || _context.Pets == null)
                {
                    return NotFound();
                }

                var pet = await _context.Pets
                    .Include(p => p.Customer)
                    .Include(p => p.Group)
                    .Where(p => p.Customer.Email == userEmail)
                    .FirstOrDefaultAsync(m => m.Id == id)
                    ;

                if (pet == null)
                {
                    return NotFound();
                }

                return View(pet);
           
        }

        // GET: Pet/Create/Customer&Admin
        public IActionResult Create()
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.CustomerId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id");
            return View();
        }

        // POST: Pet/Create/Customer&Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Diet,Breed,GroupId,CustomerId,BlobUrl")] Pet pet)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.CustomerId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;
          
            if (ModelState.IsValid)
            {
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", pet.CustomerId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", pet.GroupId);
            return View(pet);
        }

        // GET: Pet/Edit/Customer
        public async Task<IActionResult> Edit(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.CustomerId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Customer)
                .Include(p => p.Group)
                .Where(p => p.Customer.Email == userEmail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pet == null)
            {
                return NotFound();
            }
            //ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", pet.CustomerId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", pet.GroupId);
            return View(pet);
        }

        // POST: Pet/Edit/Customer&Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Diet,Breed,GroupId,CustomerId,BlobUrl")] Pet pet)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.CustomerId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;
            var admin = user?.IsAdmin;
            ViewBag.UserIsAdmin = admin;

            if (id != pet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", pet.CustomerId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", pet.GroupId);
            return View(pet);
        }

        // GET: Pet/Delete/Customer
        public async Task<IActionResult> Delete(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Customer)
                .Include(p => p.Group)
                .Where(p => p.Customer.Email == userEmail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pet/Delete/Customer&Admin
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pets == null)
            {
                return Problem("Entity set 'DogdcDBContext.Pets'  is null.");
            }
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
          return (_context.Pets?.Any(e => e.Id == id)).GetValueOrDefault();
        }




        //-------------- TÄSTÄ ALKAA ADMIN OSASTO---------------
        //------------------------------------------------------
        //------------------------------------------------------


        //GET: Pet/Index/Admin
        [Authorize(Roles = "Administrator")]
        public ActionResult IndexAdmin(string sortOrder)
        {
            ViewBag.GroupSortParm = String.IsNullOrEmpty(sortOrder) ? "group_desc" : "";
            ViewBag.CustomerSortParm = String.IsNullOrEmpty(sortOrder) ? "cust_desc" : "";
            var dogs = from s in _context.Pets
                       .Include(p => p.Customer)
                       .Include(p => p.Group)
                       select s;
            switch (sortOrder)
            {
                case "group_desc":
                    dogs = dogs.OrderBy(s => s.GroupId);
                    break;
                case "cust_desc":
                    dogs = dogs.OrderBy(s => s.Customer.LastName);
                    break;
                default:
                    dogs = dogs.OrderBy(s => s.Name);
                    break;
            }
            return View("IndexAdmin", dogs.ToList());
        }

        // GET: Pet/Details/Admin
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DetailsAdmin(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            var userId = user?.IsAdmin;
            ViewBag.UserIsAdmin = userId;

            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Customer)
                .Include(p => p.Group)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;

            if (pet == null)
            {
                return NotFound();
            }

            return View("Details", pet);

        }

        // GET: Pet/Edit/Admin
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditAdmin(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            var userId = user?.Id;
            ViewBag.CustomerId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;
            var admin = user?.IsAdmin;
            ViewBag.UserIsAdmin = admin;

            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
           
                    if (!PetExists(pet.Id))
                    {
                        return NotFound();
                    }
            return View("Edit", pet);
         
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", pet.CustomerId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", pet.GroupId);
            return View("IndexAdmin");
        }

        // GET: Pet/Delete/Admin
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAdmin(int? id)
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;

            var userId = user?.IsAdmin;
            ViewBag.UserIsAdmin = userId;

            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Customer)
                .Include(p => p.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View("Delete", pet);
        }
    }
}
