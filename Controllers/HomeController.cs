using CasusVictuz.Data;
using CasusVictuz.Models;
using CasusVictuz.VieuwModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace CasusVictuz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VictuzDb _context;

        public HomeController(ILogger<HomeController> logger, VictuzDb context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            var events = _context.Events
                .Where(e => e.Date.HasValue && e.Date.Value > DateTime.Now && e.IsAccepted) 
                .OrderBy(e => e.Date)
                .ToList();

            
            var nextEvent = events.FirstOrDefault();

            var upcomingEvents = events.Skip(1).Take(3).ToList();



            var viewModel = new HomeViewModel
            {
                NextEvent = nextEvent,
                Events = upcomingEvents
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
            {
                return View();
            }

    }
}
