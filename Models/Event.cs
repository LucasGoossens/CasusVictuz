using System;
using System.Data.Common;

namespace Casusvictuz
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; } 
        public int? Spots { get; set; } 
        public string? Location { get; set; } 
        public List<string>? Tags { get; set; } 
        public bool IsAccepted { get; set; }
        public int CategoryId { get; set; }
        public virtual required Category Category { get; set; } 
        public virtual ICollection<Registration>? Registrations { get; set; }
    }
}
