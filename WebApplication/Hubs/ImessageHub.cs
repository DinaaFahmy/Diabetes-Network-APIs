using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ModelViews;

namespace WebApplication.Hubs
{
    public interface ImessageHub
    {
     
        Task NewMessage(Message msg);
       
        Task NewMsgNotify(string userID , string username, string msg);
    }
}
