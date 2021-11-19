using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class QuestionModel
    {
        public int? ID { get; set; }
        public DateTime? Date { get; set; }
        public string Question { get; set; }
        public string UserName { get; set; }
        public List<AnswerModel> answers { get; set; }
        public int? userid { get; set; }
    }
}
