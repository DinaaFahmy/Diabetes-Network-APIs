using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class PostModel
    {
        public int? userID { get; set; }
        public int postId { get; set; }
        public string UserName { get; set; }
        public bool? role { get; set; }
        public string ReactionName { get; set; }
        public string CategoryName { get; set; }
        public string img { set; get; }
        public string content { set; get; }
        public DateTime? date { set; get; }


        public List<CommentModel>? comments { set; get; }
    }
}

