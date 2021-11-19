using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Patient
    {
        public Patient()
        {
            ChecksUps = new HashSet<ChecksUps>();
            DrugPatient = new HashSet<DrugPatient>();
            PatientDoctorsFollow = new HashSet<PatientDoctorsFollow>();
            Test = new HashSet<Test>();
        }

        public int PatientId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public short? MedicalCondetion { get; set; }
        public short? Weight { get; set; }
        public short? Height { get; set; }
        public short? LifeStyle { get; set; }
        public short? Points { get; set; }

        public virtual Users PatientNavigation { get; set; }
        public virtual ICollection<ChecksUps> ChecksUps { get; set; }
        public virtual ICollection<DrugPatient> DrugPatient { get; set; }
        public virtual ICollection<PatientDoctorsFollow> PatientDoctorsFollow { get; set; }
        public virtual ICollection<Test> Test { get; set; }
    }
}
