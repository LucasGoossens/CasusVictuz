using System;

namespace Casusvictuz
{
    public class Post
    {
        public int Id { get; set; }
        public string? Content { get; set; } 
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
