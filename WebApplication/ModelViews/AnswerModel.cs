using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class AnswerModel
    {
        public int? id { get; set; }
        public string Answer { get; set; }
        public DateTime? Date { get; set; }
        public string UserName { get; set; }
        public bool? UserType { get; set; }
        public int? userID { get; set; }
        public bool? role { get; set; }
    }
}
