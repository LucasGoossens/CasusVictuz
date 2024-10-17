using System;

namespace Casusvictuz
{
    public class Thread : Post
    {
        public string? Title { get; set; }
        public virtual List<Comment>? Comments { get; set; }

    }
}
