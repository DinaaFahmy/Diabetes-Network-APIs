using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ModelViews
{
    public class PatientProfileModel
    {
        public int userID { get; set; }
        public string user_name { get; set; }
        public string? img { get; set; }
        public int? medical_cond { get; set; }
        public DateTime? birth_date { get; set; }
        public string? gender { get; set; }
        public int? weight { get; set; }
        public int? height { get; set; }
        public int? life_style { get; set; }
        public int? access_med_info { get; set; }
        //test stuff
        public short? testType{ get; set; }
        public string? testResult { get; set; }
        public bool? testMedicationsState { get; set; }
        public DateTime? testDate { get; set; }

        //checkup stuff
        public short? checkupType { get; set; }
        public string? checkupNotes { get; set; }
        public DateTime? checkupDate { get; set; }
        public string? checkupData { get; set; }
        public short? checkupStatus { get; set; }

        //drug stuff
        public string? drugNote { get; set; }
        public int? drugDosage { get; set; }

        public virtual Drugs drug { get; set; }
    }
}
