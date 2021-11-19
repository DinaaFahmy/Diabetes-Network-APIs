using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class SavedPosts
    {
        public int post_id { get; set; }
        public int user_id { set; get; }
        public int category_id { set; get; }
        public string img { set; get; }
        public string content { set; get; }
        public int? react_id { set; get; }
        public DateTime? date { set; get; }
        public string UserName { get; set; }
        public string CategoryName { get; set; }
        public string ReactionName { get; set; }
        public bool  role { get; set; }
        public List<CommentModel>? comments { set; get; }

    }
}
