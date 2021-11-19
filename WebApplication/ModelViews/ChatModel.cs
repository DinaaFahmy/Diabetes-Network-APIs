using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ModelViews
{
    public class ChatModel
    {
        public int userId { get; set; }
        public int chatId { get; set; }
        public string userName { get; set; }
        public List<Msg> Msgs { get; set; }
    }
}
