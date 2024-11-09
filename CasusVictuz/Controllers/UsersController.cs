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

            var EmailLogin = model.Name;
            var user = _context.Users.FirstOrDefault(c => (c.Name == model.Name || c.Email == model.Name) && c.Password == model.Password);

            if (user != null)
            {
                // claims worden de ingelogde gegevens van de gebruiker in opgeslagen (naam en id)
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("IsMember", user.IsMember ? "True" : "False"),
            new Claim("IsAdmin", user.IsAdmin ? "True" : "False"),
            new Claim("IsGuest", (bool)user.IsGuest ? "True" : "False")
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
        public async Task<IActionResult> Create([Bind("Id,Name, Email, Password")] User user)
        {
            // Check if the name or password exceeds 20 characters
            if (user.Name.Length > 20)
            {
                ModelState.AddModelError("Name", "De naam kan niet langer zijn dan 20 tekens");
                return View(user);
            }

            if (user.Password.Length > 20)
            {
                ModelState.AddModelError("Password", "Het wachtwoord kan niet langer zijn dan 20 tekens");
                return View(user);
            }


            // Check if the username already exists in the database
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
            if (existingUser != null)
            {
                ModelState.AddModelError("Name", "Deze gebruikersnaam bestaat al");
                return View(user);
            }

            if (!user.Email.Contains('@'))
            {
                ModelState.AddModelError("Email", "Email is niet valide");
                return View(user);
            }

            if (EmailExists(user.Email))
            {
                ModelState.AddModelError("Email", "Email is al in gebruik");
                return View(user);
            }


            if (ModelState.IsValid)
            {
                user.IsGuest = false;
                user.IsMember = true;
                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Account aangemaakt! Log in om je account te verifiëren.";
                return RedirectToAction(nameof(Login));
            }

            // Fallback in case of other validation errors
            TempData["ErrorMessage"] = "Er is iets fout gegaan. Controleer je invoer en probeer het opnieuw.";
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMemberAccountFromGuestAcc(int loggedInUserId, string email, string password)
        {            
            var guestAcc = await _context.Users.FirstOrDefaultAsync(u => u.Id == loggedInUserId);
         
            if (guestAcc != null)
            {
                guestAcc.Email = email;
                guestAcc.Password = password;
                guestAcc.IsMember = true;
                guestAcc.IsGuest = false;

                await _context.SaveChangesAsync();

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, guestAcc.Name),
            new Claim(ClaimTypes.NameIdentifier, guestAcc.Id.ToString()),
            new Claim("IsMember", guestAcc.IsMember ? "True" : "False"),
            new Claim("IsGuest", (bool)guestAcc.IsGuest ? "True" : "False"),
            new Claim("IsAdmin", guestAcc.IsAdmin ? "True" : "False")
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Claims worden opgeslagen met cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                return RedirectToAction("Details", "Users", new { id = guestAcc.Id });
            }
            
            return NotFound();
        }


        // GET: Users/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }

        // GET: Users1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,IsAdmin,IsMember")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AllUsers));
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool EmailExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }


        public async Task<IActionResult> AllUsers()
        {
            return View(await _context.Users.ToListAsync());
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllUsers));
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
