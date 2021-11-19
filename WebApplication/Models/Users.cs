using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public partial class Users
    {
        public Users()
        {
            Answers = new HashSet<Answers>();
            Chat = new HashSet<Chat>();
            Msg = new HashSet<Msg>();
            Questions = new HashSet<Questions>();
            UserSavedPosts = new HashSet<UserSavedPosts>();
            UserSavedQuestion = new HashSet<UserSavedQuestion>();
        }

        // public string PhoneNumber { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        // public string Email { get; set; }
        //  public string Password { get; set; }

         public bool? Type { get; set; }

        public string ImageSource { get; set; }

        [ForeignKey("ID")]
        public virtual ApplicationUser User { get; set; }
        public string ID { get; set; }
        public virtual Doctor Doctor { get; set; }

       
        public virtual Patient Patient { get; set; }
        public virtual Posts Posts { get; set; }
        public virtual ICollection<Answers> Answers { get; set; }
        public virtual ICollection<Chat> Chat { get; set; }
        public virtual ICollection<Msg> Msg { get; set; }
        public virtual ICollection<Questions> Questions { get; set; }
        public virtual ICollection<UserSavedPosts> UserSavedPosts { get; set; }
        public virtual ICollection<UserSavedQuestion> UserSavedQuestion { get; set; }
    }
}
