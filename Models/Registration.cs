using System.ComponentModel.DataAnnotations;

namespace Casusvictuz
{
    public class Registration
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual required User? User { get; set; }
        [Required]
        public int EventId { get; set; }
        public virtual required Event? Event { get; set; }
        public bool IsOrganised { get; set; }
    }
}
