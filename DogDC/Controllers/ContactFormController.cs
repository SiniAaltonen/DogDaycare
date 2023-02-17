using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DogDC.Models;

namespace DogDC.Controllers
{
    public class ContactFormController : Controller
    {
        private readonly DogdcDBContext _context;

        public ContactFormController(DogdcDBContext context)
        {
            _context = context;
        }

        // GET: ContactForm
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
              return _context.ContactForms != null ? 
                          View(await _context.ContactForms
                          .ToListAsync()):
                          Problem("Entity set 'DogdcDBContext.ContactForms'  is null.");
        }

        // GET: ContactForm/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContactForms == null)
            {
                return NotFound();
            }

            var contactForm = await _context.ContactForms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactForm == null)
            {
                return NotFound();
            }

            return View(contactForm);
        }

        //// GET: ContactForm/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ContactForm/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Email,Subject,Message,Timestamp")] ContactForm contactForm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(contactForm);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(contactForm);
        //}

        //// GET: ContactForm/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.ContactForms == null)
        //    {
        //        return NotFound();
        //    }

        //    var contactForm = await _context.ContactForms.FindAsync(id);
        //    if (contactForm == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(contactForm);
        //}

        //// POST: ContactForm/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Subject,Message,Timestamp")] ContactForm contactForm)
        //{
        //    if (id != contactForm.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(contactForm);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ContactFormExists(contactForm.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(contactForm);
        //}

        // GET: ContactForm/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContactForms == null)
            {
                return NotFound();
            }

            var contactForm = await _context.ContactForms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactForm == null)
            {
                return NotFound();
            }

            return View(contactForm);
        }

        // POST: ContactForm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContactForms == null)
            {
                return Problem("Entity set 'DogdcDBContext.ContactForms'  is null.");
            }
            var contactForm = await _context.ContactForms.FindAsync(id);
            if (contactForm != null)
            {
                _context.ContactForms.Remove(contactForm);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactFormExists(int id)
        {
          return (_context.ContactForms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
