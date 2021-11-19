using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Drugs
    {
        public Drugs()
        {
            DrugPatient = new HashSet<DrugPatient>();
        }

        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string ImageSource { get; set; }
        public string DosageType { get; set; }

        public virtual ICollection<DrugPatient> DrugPatient { get; set; }
    }
}
