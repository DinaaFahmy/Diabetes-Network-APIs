using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class DrugPatient
    {
        public int DrugId { get; set; }
        public string Note { get; set; }
        public int PatientId { get; set; }
        public int? Dosage { get; set; }

        public virtual Drugs Drug { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
