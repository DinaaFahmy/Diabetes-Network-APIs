using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class FollowingPatients
    {
        public int userID { get; set; }
        public string user_name { get; set; }
        public string img { get; set; }
        public int? medical_cond { get; set; }
        public DateTime? birth_date { get; set; }
        public string gender { get; set; }
        public int? weight { get; set; }
        public int? height { get; set; }
        public int? life_style { get; set; }
        public int access_med_info { get; set; }
    }

}
