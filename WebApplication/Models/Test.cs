using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Test
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Result { get; set; }
        public short Type { get; set; }
        public int? PatientId { get; set; }
        public bool? Medication { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
