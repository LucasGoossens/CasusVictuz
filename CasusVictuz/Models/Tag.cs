using System.ComponentModel.DataAnnotations;

namespace CasusVictuz.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public int EventId { get; set; }
    }
}
