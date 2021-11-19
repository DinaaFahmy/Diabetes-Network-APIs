using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; }
        public string NotificationContent { get; set; }
        public short? Type { get; set; }
        /**************************************************************/
        //notification answer
        public int? AnswerId { get; set; }
        public string Answer { get; set; }
        public int QuestionId { get; set; }
        public DateTime? QuestionDate { get; set; }
        public string Question { get; set; }
        /*****************************************************************/
        /// <summary>
        /// ///////////////////////////
        /// </summary>
        //public int? userID_asked { get; set; }
        public string userName { get; set; }
        //notification asked  (mentioned)
        public int? MentionId { get; set; } //doctor
        //notification follow
        public int? FollowId { get; set; } //doctor
        public int? userID_follower { get; set; }
        public short? AccessMedicalInfo { get; set; }

        //notification medical info
        public int? MedicalInfoId { get; set; } //patient

    }
}
