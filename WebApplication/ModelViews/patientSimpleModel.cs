using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class patientSimpleModel
    {
        public string? userName { get; set; }
        //public string? email { get; set; }
        public int id { get; set; }
        public string imagesource { get; set; }
        public short accessMedicalInfo { get; set; }
    }
}
