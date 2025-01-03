using System;
using System.Reflection.Metadata;

namespace Casusvictuz
{
    public class Comment : Post
    {        
        public int ThreadId { get; set; }
        public virtual required Thread Thread { get; set; }
        public int? ParentCommentId { get; set; }
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Comment>? Replies { get; set; }

    }
}
