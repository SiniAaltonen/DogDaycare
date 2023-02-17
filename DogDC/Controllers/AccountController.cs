using DogDC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogDC.Controllers
{
    public class AccountController : Controller
    {
        private readonly DogdcDBContext _context;

        public AccountController( DogdcDBContext context)
        {
            _context = context;
        }
        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public IActionResult UserProfile()
        {
            string email = User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
            var customerID = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            
            return View(new UserProfile()
            {
                Name = User.Identity.Name,
                Email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                Avatar = User.FindFirst(c => c.Type == "picture")?.Value,
                Customer = _context.Customers.FirstOrDefault(s => s.Email == email),
                CustomerId = _context.Customers.FirstOrDefault(u => u.Email == customerID).Id
            }) ;

        }

        public IActionResult Index()
        {
            var userEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Customers.FirstOrDefault(u => u.Email == userEmail);
            ViewBag.userId = _context.Customers.FirstOrDefault(u => u.Email == userEmail)?.Id;
            return View(user);
        }

        public IActionResult AddUserToDB(string? email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]

        public IActionResult AddUserToDB([FromForm] Customer user)
        {
            _context.Customers.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        
        }


        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
