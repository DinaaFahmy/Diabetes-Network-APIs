using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class ChecksUps
    {
        public int? Id { get; set; }
        public short CheckupType { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public string ResultData { get; set; }
        public short? Status { get; set; }
        public int? PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
