using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Certificates = new HashSet<Certificates>();
            PatientDoctorsFollow = new HashSet<PatientDoctorsFollow>();
            QuestionDoctorsMention = new HashSet<QuestionDoctorsMention>();
        }

        public int DoctorId { get; set; }
        public string Address { get; set; }
        public bool ValidationStatus { get; set; }

        public virtual Users DoctorNavigation { get; set; }
        public virtual ICollection<Certificates> Certificates { get; set; }
        public virtual ICollection<PatientDoctorsFollow> PatientDoctorsFollow { get; set; }
        public virtual ICollection<QuestionDoctorsMention> QuestionDoctorsMention { get; set; }
    }
}
