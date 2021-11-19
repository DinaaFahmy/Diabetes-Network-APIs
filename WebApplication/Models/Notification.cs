using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string NotificationContent { get; set; }
        public short? Type { get; set; }

        public virtual NotificationAnswer NotificationAnswer { get; set; }
        public virtual NotificationAsked NotificationAsked { get; set; }
        public virtual NotificationFollow NotificationFollow { get; set; }
        public virtual NotificationMedicalInfo NotificationMedicalInfo { get; set; }
    }
}
