using DogDC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace DogDC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DogdcDBContext _context;

        public HomeController(ILogger<HomeController> logger, DogdcDBContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {

            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            if (User.Identity.IsAuthenticated)

                if (!_context.Customers.Where(s => s.Email == email).Any())
                {
                    Debug.WriteLine("Need to create a user.");
                    return RedirectToAction("AddUserToDB", "Account", new { email = email });
                }

            return View();
        }

        // GET: Home/Contact
        public IActionResult Contact()
        {
            return View();
        }

        // POST: Home/Contact
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Id,Name,Email,Subject,Message,Timestamp")] ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactForm);
        }

        //[Authorize(Roles = "Administrator")]
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Koirapaivakoti()
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