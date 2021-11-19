using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class UserSavedQuestion
    {
        public int QuestionId { get; set; }
        public int UserId { get; set; }

        public virtual Questions Question { get; set; }
        public virtual Users User { get; set; }
    }
}
