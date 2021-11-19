using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public partial class Chat
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }

        public virtual Msg ChatNavigation { get; set; }
        public virtual Users User { get; set; }
    }
}
