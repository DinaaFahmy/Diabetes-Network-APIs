using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Answers
    {
        public Answers()
        {
            NotificationAnswer = new HashSet<NotificationAnswer>();
        }

        public int AnswerId { get; set; }
        public int? QuestionId { get; set; }
        public string Answer { get; set; }
        public DateTime? Date { get; set; }
        public int UserId { get; set; }

        public virtual Questions Question { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<NotificationAnswer> NotificationAnswer { get; set; }
    }
}
