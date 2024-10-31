using System.ComponentModel.DataAnnotations;
namespace Casusvictuz
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Password { get; set; } 
        public bool IsAdmin { get; set; }
        public bool IsMember { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Registration>? Registrations { get; set; }

    }
}
