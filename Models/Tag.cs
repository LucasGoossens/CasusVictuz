using System.ComponentModel.DataAnnotations;

namespace CasusVictuz.Models
{
    public class Tag
    {
        int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public int EventId { get; set; }
    }
}
