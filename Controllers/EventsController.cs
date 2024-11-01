using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasusVictuz.Data;
using Casusvictuz;
using System.Security.Claims;
using CasusVictuz.VieuwModels;
using CasusVictuz.Models;

namespace CasusVictuz.Controllers
{
    public class EventsController : Controller
    {
        private readonly VictuzDb _context;

        public EventsController(VictuzDb context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> IndexUser(string? searchString = null)
        {
            var victuzDb = _context.Events
                .Include(e => e.Tags)
                .Include(e => e.Category)
                .Include(e => e.Registrations)
                .Where(e => e.IsAccepted == true)
                .Where(e => e.Date >= DateTime.Now);

            if (!string.IsNullOrEmpty(searchString))
            {
                victuzDb = victuzDb.Where(e =>
                   e.Name.Contains(searchString) ||
                   e.Category.Title.Contains(searchString) ||
                   e.Tags.Any(t => t.Name.Contains(searchString))
               );
            }

            victuzDb = victuzDb.OrderBy(e => e.Date);

            return View(await victuzDb.ToListAsync());
        }

        public async Task<IActionResult> IndexAdmin()
        {
            var victuzDb = _context.Events.Include(e => e.Category)
            .Where(e => e.IsAccepted);
            return View(await victuzDb.ToListAsync());
        }

        public async Task<IActionResult> SuggestionIndex()
        {
            var victuzDb = _context.Events.Include(e => e.Category)
            .Where(e => !e.IsAccepted);
            return View(await victuzDb.ToListAsync());
        }

        // GET: Events/Details/5
        // GET: Events/Details/5
        public async Task<IActionResult> DetailsAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
               .Include(e => e.Category)
               .Include(e => e.Registrations)
               .ThenInclude(r => r.User)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            var registeredUsers = @event.Registrations.Select(r => r.User).ToList();
            var firstOrganizerRegistrations = @event.Registrations.FirstOrDefault(r => r.IsOrganised);
            var firstOrganizer = firstOrganizerRegistrations?.User?.Name;

            var viewModel = new DetailsEventViewModel
            {
                Event = @event,
                RegisteredUsers = registeredUsers,
                FirstOrganizer = firstOrganizer ?? "onbekend"

            };
            return View(viewModel); // Ensure you're returning the correct model


        }


        public async Task<IActionResult> DetailsUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Name,Description,Spots,Location,IsAccepted,CategoryId,UrlLinkPicture")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", @event.CategoryId);
            return View(@event);
        }
        // GET: Events/CreateAdmin
        public IActionResult CreateAdmin()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Events/CreateAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin([Bind("Id,Date,Name,Description,Spots,Location,IsAccepted,CategoryId,UrlLinkPicture")] Event @event, string[] Tags)
        {
            if (ModelState.IsValid)
            {
                @event.IsAccepted = true;
                _context.Add(@event);
                await _context.SaveChangesAsync();

                // Assuming you have a way to get the current admin user ID
                var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Create a new Registration record for the admin
                var registration = new Registration
                {
                    UserId = int.Parse(adminUserId),
                    User = await _context.Users.FindAsync(int.Parse(adminUserId)), // Set the User property
                    EventId = @event.Id,
                    Event = @event, // Set the Event property
                    IsOrganised = true
                };

                _context.Registrations.Add(registration);

                // Add tags to the event
                foreach (var tagName in Tags)
                {
                    var tag = new Tag
                    {
                        Name = tagName,
                        EventId = @event.Id
                    };
                    _context.Tags.Add(tag);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(IndexAdmin));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", @event.CategoryId);
            return View(@event);
        }


        public IActionResult CreateSuggestion()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSuggestion([Bind("Date,Name,Description,Spots,Location,CategoryId, UrlLinkPicture")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.IsAccepted = false;
                _context.Add(@event);
                await _context.SaveChangesAsync();

                var suggestorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var registration = new Registration
                {
                    
                    UserId = int.Parse(suggestorUserId),
                    User = null,
                    EventId = @event.Id,
                    Event = null,
                    IsOrganised = true
                };

                _context.Registrations.Add(registration);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(IndexUser));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", @event.CategoryId);
            return View(@event);
        }


        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Tags) // Include the Tags
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", @event.CategoryId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name,Description,Spots,Location,IsAccepted,CategoryId,UrlLinkPicture")] Event @event, string[] Tags)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the event
                    _context.Update(@event);

                    // Update the tags
                    var existingTags = await _context.Tags.Where(t => t.EventId == id).ToListAsync();
                    _context.Tags.RemoveRange(existingTags);

                    foreach (var tagName in Tags)
                    {
                        var tag = new Tag { Name = tagName, EventId = id };
                        _context.Tags.Add(tag);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexUser));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAdmin));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }



        [ActionName("Register")]
        public IActionResult Register(int eventId)
        {
            // check of de gebruiker is ingelogd
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("Je moet inloggen voor je voor een evenement kan inschrijven");
            }

            int customerId = int.Parse(userId);

            // Controleer of de gebruiker al is geregistreerd voor dit evenement
            var alreadyRegistered = _context.Registrations.Any(r => r.UserId == customerId && r.EventId == eventId);
            if (alreadyRegistered)
            {
                return BadRequest("Je bent al geregistreerd voor dit evenement");
            }

            // Controleer of het evenement volgeboekt is
            var eventItem = _context.Events.Include(e => e.Registrations).FirstOrDefault(e => e.Id == eventId);
            if (eventItem == null)
            {
                return BadRequest("Event null");
            }

            if (eventItem.Registrations.Count >= eventItem.Spots)
            {
                return BadRequest("Dit evenement is al vol");
            }

            // Maak een nieuwe registratie aan
            var newRegistration = new Registration
            {
                UserId = customerId,
                User = null, // gebruiker word al opgehaald in de controller en opgeslagen met UserId
                EventId = eventId,
                Event = null, // event word al opgehaald in de controller en opgeslagen met EventId
                IsOrganised = false // inschrijving is geen organisator
            };

            _context.Registrations.Add(newRegistration);
            _context.SaveChanges();

            return RedirectToAction("DetailsUser", new { id = eventId });
        }


        [ActionName("Unregister")]        
        public async Task<IActionResult> UnregisterUserForEvent(int id)
        {
            System.Diagnostics.Debug.WriteLine(id);            
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                _context.Registrations.Remove(registration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexUser));
        }

    }
}
