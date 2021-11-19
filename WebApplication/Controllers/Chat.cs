using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication.Hubs;
using WebApplication.Models;
using WebApplication.ModelViews;
using WebApplication.Repo;
namespace WebApplication.Controllers {
    [Route ("[controller]")]
    [ApiController]
    public class Chat : ControllerBase {
        public ImessageHub messageHub;

        private int user_id;
        private UserManager<ApplicationUser> manager;
        private IDiabetes diabetes;
        private DiabetesSystem2Context db;
        public Chat (UserManager<ApplicationUser> _manager, IDiabetes _diabetes, DiabetesSystem2Context _db, ImessageHub m) {
            manager = _manager;
            diabetes = _diabetes;
            db = _db;
            messageHub = m;

        }
        private void GetUserId () {
            user_id = diabetes.GetUserID (manager.GetUserId (HttpContext.User));
        }

        [HttpGet]
        [Route ("chat/{id}")]
        [Authorize]
        public IActionResult GetChat (int id) {
            GetUserId ();

            var x = diabetes.GetMsgInChat (user_id, id);
            if (x != null) {
                return Ok (x);
            }
            return NotFound ();

        }

        [HttpPost]
        [Route ("chat/{id}/{recivedid}")]
        [Authorize]
        public IActionResult PostChat (int id, int recivedid, [FromBody] string msg) {
            GetUserId ();

            var y = db.users.Where (a => a.UserId == recivedid).ToList ().FirstOrDefault ();

            var u = db.users.Where (a => a.UserId == user_id).ToList ().FirstOrDefault ();
            var x = diabetes.AddMsg (msg, id, user_id);
            messageHub.NewMsgNotify (y.ID, u.UserName, msg);

            return Ok (x);
        }

        [HttpGet]
        [Route ("name/{id}")]
        [Authorize]
        public string getName (int id) {
            GetUserId ();
            var x = db.users.Where (a => a.UserId == id).Select (a => a.UserName).FirstOrDefault ();
            return JsonConvert.SerializeObject (x);
        }

        [HttpGet]
        [Route ("chat")]
        [Authorize]

        public IActionResult getChats () {
            GetUserId ();
            var x = diabetes.GetMyChats (user_id);
            return Ok (x);
        }
    }
}