using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class CommentModel
    {
        public int ID { get; set; }

        public int PostID { get; set; }
        public string  UserName { get; set; }

        public int? UserID { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
