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
        public async Task<IActionResult> IndexUser()
        {
            var victuzDb = _context.Events.Include(e => e.Category)
            .Where(e => e.IsAccepted == true)
            .Where(e => e.Date >= DateTime.Now)
            .OrderBy(e => e.Date);
            return View(await victuzDb.ToListAsync());
        }

        public async Task<IActionResult> IndexAdmin()
        {
            var victuzDb = _context.Events.Include(e => e.Category);
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event); // Ensure you're returning the correct model
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
        public async Task<IActionResult> Create([Bind("Id,Date,Name,Description,Spots,Location,IsAccepted,CategoryId")] Event @event)
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

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name,Description,Spots,Location,IsAccepted,CategoryId")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
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
                return RedirectToAction(nameof(IndexAdmin));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", @event.CategoryId);
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
                return BadRequest("Je bent al gerigistreerd voor dit evenement");
            }

            // Controleer of het evenement volgeboekt is
            var eventItem = _context.Events.Include(e => e.Registrations).FirstOrDefault(e => e.Id == eventId);
            if (eventItem == null || eventItem.Registrations.Count >= eventItem.Spots)
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

    }
}
