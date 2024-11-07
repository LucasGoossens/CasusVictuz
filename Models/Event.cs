using CasusVictuz.Models;
using System;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace Casusvictuz
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public required string Name { get; set; } 
        public string? Description { get; set; } 
        public int? Spots { get; set; }
        [Required]
        public int? LocationId { get; set; }
        public virtual required Location? Location { get; set; }
        public bool IsAccepted { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual required Category? Category { get; set; } 
        public virtual ICollection<Registration>? Registrations { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
        public string? UrlLinkPicture { get; set; }
    }
}
