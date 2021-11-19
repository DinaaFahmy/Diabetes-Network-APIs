using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class QuestionDoctorsMention
    {
        public QuestionDoctorsMention()
        {
            NotificationAsked = new HashSet<NotificationAsked>();
        }

        public int QuestionId { get; set; }
        public int Id { get; set; }
        public bool? Status { get; set; }
        public int DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Questions Question { get; set; }
        public virtual ICollection<NotificationAsked> NotificationAsked { get; set; }
    }
}
