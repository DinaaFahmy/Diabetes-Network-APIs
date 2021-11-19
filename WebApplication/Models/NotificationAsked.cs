using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class NotificationAsked
    {
        public int MentionId { get; set; }
        public int NotificationId { get; set; }

        public virtual QuestionDoctorsMention Mention { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
