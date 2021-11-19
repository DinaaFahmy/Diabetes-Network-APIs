using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Comments
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Post")]

        public int PostID { get; set; }
        [ForeignKey("User")]

        public int? UserID { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public virtual Users User { get; set; }
        public virtual Posts Post { get; set; }
    }
}
