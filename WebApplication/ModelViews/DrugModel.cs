using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class DrugModel 
    {
        public int drugId { get; set; }
        public string note { get; set; }
        public int patientId { get; set; }
        public int? dosage { get; set; }
        public string drugName { get; set; }
        public string imageSource { get; set; }
    }
}
