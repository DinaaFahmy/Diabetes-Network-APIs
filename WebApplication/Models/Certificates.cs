using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Certificates
    {
        public string Certificate { get; set; }
        public string University { get; set; }
        public int DoctorId { get; set; }
        public int Id { get; set; }
        
        public virtual Doctor Doctor { get; set; }
    }
}
