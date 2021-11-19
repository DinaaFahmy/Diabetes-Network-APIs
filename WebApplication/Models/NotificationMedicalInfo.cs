using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class NotificationMedicalInfo
    {
        public int MedicalInfoId { get; set; }
        public int NotificationId { get; set; }

        public virtual PatientDoctorsFollow MedicalInfo { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
