using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Reactions
    {
        public Reactions()
        {
            Posts = new HashSet<Posts>();
        }

        public int ReactionId { get; set; }
        public string ReactionName { get; set; }

        public virtual ICollection<Posts> Posts { get; set; }
    }
}
