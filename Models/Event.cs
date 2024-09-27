using System;

namespace Casusvictuz
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; } 
        public string? Spots { get; set; } 
        public string? Location { get; set; } 
        public List<string>? Tags { get; set; } 
        public bool IsAccepted { get; set; }
    }
}
