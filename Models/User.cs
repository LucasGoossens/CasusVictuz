using System;

namespace Casusvictuz
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; } 
        public bool IsAdmin { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Registration>? Registrations { get; set; }
    }
}
