using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class PatientDoctorsFollow
    {
        public PatientDoctorsFollow()
        {
            NotificationMedicalInfo = new HashSet<NotificationMedicalInfo>();
        }

        public int? PatienId { get; set; }
        public int? DoctorId { get; set; }
        public int Id { get; set; }
        public short AccessMedicalInfo { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patien { get; set; }
        public virtual ICollection<NotificationMedicalInfo> NotificationMedicalInfo { get; set; }
    }
}
