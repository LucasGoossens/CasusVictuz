using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasusVictuz.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using CasusVictuz.VieuwModels;
using Casusvictuz;


namespace CasusVictuz.Controllers
{
    public class UsersController : Controller
    {
        private readonly VictuzDb _context;

        public UsersController(VictuzDb context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var user = _context.Users.FirstOrDefault(c => c.Name == model.Name && c.Password == model.Password);

            if (user != null)
            {
                // claims worden de ingelogde gegevens van de gebruiker in opgeslagen (naam en id)
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("IsMember", user.IsMember ? "True" : "False"),
            new Claim("IsAdmin", user.IsAdmin ? "True" : "False")
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Claims worden opgeslagen met cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }
        // GET: Users/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Registrations) 
                .Include(u => u.Posts)         
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

    }

}

/*       Uitleg quotes
 
         // Krijg huidige naam uit claims
        var name = User.FindFirst(ClaimTypes.Name)?.Value;

        // Krijg huidige rol uit claims (admin = true, gebruiker = falls)
        bool isAdmin = User.HasClaim("IsAdmin", "True");

        // Krijg huidig id uit claims (alleen gebruiken als je weet dat er is ingelogd)
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        // Krijg huidig id uit claims (kan als er geen gebruiker is ingelogd)
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            customerId = int.Parse(userId);
        }

        // Check of de user is ingelogd
        bool isLoggedIn = HttpContext.User.Identity.IsAuthenticated;
 */
