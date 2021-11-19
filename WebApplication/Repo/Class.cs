using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.ModelViews;

namespace WebApplication.Repo {
    public partial class DiabetesRepo : IDiabetes {

        Patient IDiabetes.GetPatient (int ID) {
            return db.Patient.AsNoTracking ().Where (a => a.PatientId == ID).FirstOrDefault ();

        }

        Patient IDiabetes.UpdatePatient (Patient patient) {
            var p = db.Patient.Update (patient).Entity;
            db.SaveChanges ();
            return p;
        }

        List<Test> IDiabetes.GetAllTests (int PatientID) {
            return db.Test.FromSqlRaw ("SelectTests {0}", PatientID).ToList ();
        }

        List<Msg> IDiabetes.GetMsgInChat (int myID, int UserID)

        {
            List<Msg> Msgs = new List<Msg> ();
            Chat x = new Chat ();
            var c = (from c1 in db.Chat join c2 in db.Chat on c1.ChatId equals c2.ChatId where c1.UserId == myID && c2.UserId == UserID select c1).ToList ().FirstOrDefault ();
            if (c == null) {
                c = db.Chat.FromSqlRaw (" EXEC Chat_Insert {0},{1}", myID, UserID).ToList<Chat> ().FirstOrDefault ();

            } else {
                var y = db.Msg.Where (a => a.ChatId == c.ChatId).OrderBy (a => a.Date).ToList ();
                Msgs = y;
            }
            if (Msgs.Count == 0) {
                Msgs.Add (new Msg () { ChatId = c.ChatId, MsgId = -1 });
            }

            return Msgs;

        }

        List<ChatModel> IDiabetes.GetMyChats (int myID) {
            var x = db.Chat.Where (a => a.UserId == myID).Distinct ().ToList ();
            var y = db.Chat.Where (a => a.UserId == myID).Select (a => a.ChatId).ToList ();
            var c = db.Msg.Where (a => y.Contains (a.ChatId)).ToList ().OrderByDescending (a => a.Date).Select (a => a.ChatId).Distinct ().ToArray ();
            List<ChatModel> chats = new List<ChatModel> ();
            for (int i = 0; i < c.Count (); i++) {
                var chat = new ChatModel ();
                chat.chatId = c[i];
                chat.userId = db.Chat.Where (a => a.ChatId == c[i] && a.UserId != myID).ToList ().FirstOrDefault ().UserId;
                chat.userName = db.users.Where (a => a.UserId == chat.userId).ToList ().FirstOrDefault ().UserName;
                chat.Msgs = db.Msg.Where (a => a.ChatId == c[i]).OrderBy (a => a.Date).ToList ();
                chats.Add (chat);
            }
            return chats;
        }

    }
}