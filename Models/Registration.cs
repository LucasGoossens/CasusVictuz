using System;

namespace Casusvictuz
{
    public class Registration
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual required User User { get; set; }
        public int EventId { get; set; }
        public virtual required Event Event { get; set; }
        public bool IsOrganised { get; set; }
    }
}
