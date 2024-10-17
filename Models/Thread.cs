using System;
using System.ComponentModel.DataAnnotations;

namespace Casusvictuz
{
    public class Thread : Post
    {
        [Required]
        public required string Title { get; set; }
        public virtual List<Comment>? Comments { get; set; }

    }
}
