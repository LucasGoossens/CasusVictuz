using System;
using System.ComponentModel.DataAnnotations;

namespace Casusvictuz
{
    public class Post
    {
        
        public int Id { get; set; }
        [Required]
        public required string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int UserId { get; set; }        
        public virtual required User User { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual required Category Category { get; set; }
    }
}
