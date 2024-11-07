using System.ComponentModel.DataAnnotations;
namespace Casusvictuz
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naam is verplicht")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Email is verplicht")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        public required string Password { get; set; } 
        public bool IsAdmin { get; set; }
        public bool IsMember { get; set; }
        public bool? IsGuest { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Registration>? Registrations { get; set; }

    }
}
