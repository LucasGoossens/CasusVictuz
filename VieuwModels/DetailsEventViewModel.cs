using Casusvictuz;

namespace CasusVictuz.VieuwModels
{
    public class DetailsEventViewModel
    {
        public Event Event { get; set; }
        public List<User> Users { get; set; }
        public List<User> UserNotInEvent { get; set; }
        public List<Registration> Registrations { get; set; }
        public List<User> RegisteredUsers { get; set; } // Voeg deze regel toe
    }
}
