using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class NotificationFollow
    {
        public int FollowId { get; set; }
        public int NotificationId { get; set; }

        public virtual Notification Notification { get; set; }
    }
}
