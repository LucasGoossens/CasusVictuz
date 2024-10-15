using Casusvictuz;
using Microsoft.EntityFrameworkCore;


namespace CasusVictuz.Data
{
    public class VictuzDb : DbContext
    {
        // is niet af, slechs een eerste opzet
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Registration> Registrations { get; set; } = null!;
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; } = null!;


    }
}
