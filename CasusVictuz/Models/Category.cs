using System.ComponentModel.DataAnnotations;

namespace Casusvictuz
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; } 
    }
}
