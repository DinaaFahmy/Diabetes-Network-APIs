using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Posts
    {
        public Posts()
        {
            UserSavedPosts = new HashSet<UserSavedPosts>();
        }

        public int PostId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string ImageSource { get; set; }
        public string PostContent { get; set; }
        public int? ReactionId { get; set; }
        public DateTime? PostDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual Users Post { get; set; }
        public virtual Reactions Reaction { get; set; }
        public virtual ICollection<UserSavedPosts> UserSavedPosts { get; set; }
    }
}
