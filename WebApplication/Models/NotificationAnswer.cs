using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class NotificationAnswer
    {
        public int AnswerId { get; set; }
        public int NotificationId { get; set; }

        public virtual Answers Answer { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
