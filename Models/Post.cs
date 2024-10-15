using System;

namespace Casusvictuz
{
    public class Post
    {
        public int Id { get; set; }
        public string? Content { get; set; } 
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public virtual required User User { get; set; }
        public int CategoryId { get; set; }
        public virtual required Category Category { get; set; }
    }
}
