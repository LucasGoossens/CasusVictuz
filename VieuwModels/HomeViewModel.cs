using Casusvictuz;
using CasusVictuz.Models;

namespace CasusVictuz.VieuwModels
{
    public class HomeViewModel
    {
        public Event? NextEvent { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();
    }
}
