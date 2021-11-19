using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.ModelViews;
using WebApplication.Repo;

namespace WebApplication.Controllers {
    [Route ("[controller]")]
    [ApiController]
    [Authorize]
    public partial class restController : ControllerBase {
        private int user_id;
        private UserManager<ApplicationUser> manager;
        private IDiabetes diabetes;
        private DiabetesSystem2Context db;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public restController (UserManager<ApplicationUser> _manager, IDiabetes _diabetes, DiabetesSystem2Context _db, SignInManager<ApplicationUser> mange) {
            manager = _manager;
            diabetes = _diabetes;
            db = _db;
            _signInManager = mange;
        }

        private void GetUserId () {
            user_id = diabetes.GetUserID (manager.GetUserId (HttpContext.User));
        }

     

        [Route ("ctg")]
        ///GetAllCategories
        public IActionResult ctg () {
            var c = diabetes.GetAllCategories ();
            return Ok (c);
        }

       

      

       

        //-------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Route ("allPosts")]
        [AllowAnonymous]
        public IActionResult all_posts () {
            //var n = diabetes.GetAllPosts();
            //return Ok(n);
            List<Posts> posts = diabetes.GetAllPosts ();
            List<PostModel> postM = new List<PostModel> ();
            List<Reactions> reactions = diabetes.GetAllReacts ();
            List<Category> categories = diabetes.GetAllCategories ();
            List<Users> users = db.users.Select (a => a).ToList ();

            for (int i = 0; i < posts.Count; i++) {
                PostModel p = new PostModel ();
                p.userID = posts[i].UserId;
                p.CategoryName = categories.FirstOrDefault (a => a.CategoryId == posts[i].CategoryId).CategoryName;
                p.ReactionName = reactions.FirstOrDefault (a => a.ReactionId == posts[i].ReactionId).ReactionName;
                p.img = posts[i].ImageSource;
                p.date = posts[i].PostDate;
                p.UserName = diabetes.GetUser (posts[i].UserId).UserName;
                p.content = posts[i].PostContent;
                p.postId = posts[i].PostId;
                p.comments = diabetes.GetComments (posts[i].PostId);
                p.role = users.FirstOrDefault (a => a.UserId == posts[i].UserId).Type;
                postM.Add (p);
            }

            return Ok (postM);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        //1 get dr info
       

        //5 get dr posts

        //6 get dr questions
        ///////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        [Route ("myfollowers/{pid}")]
        [Authorize]
        public IActionResult getOneOfThePatients (int pid) {
            GetUserId ();
            var n = diabetes.getAllPatients (pid);
            return Ok (n);
        }

        /// <summary>
        /// //////////////////////////

        [HttpPost]
        [Route ("updatephonenumber")]
        [Authorize]
        public IActionResult updatephonenumber ([FromBody] string phone) //gives error in function
        {
            diabetes.updatephonenumber (manager.GetUserId (HttpContext.User), phone);

            return Ok ();
        }

     

        [HttpGet ("{ID}")]
        [Route ("Comments/{ID}")]
        [Authorize]
        public IActionResult GetComments (int ID) {
            GetUserId ();
            var Comments = diabetes.GetComments (ID);
            return Ok (Comments);
        }

        [HttpPost]
        [Route ("Comments")]
        [Authorize]
        public IActionResult AddComments ([FromBody] Comments comment) {
            GetUserId ();
            comment.UserID = user_id;
            var c = diabetes.AddComment (comment);

            return Ok (c);
        }

        [HttpDelete]
        [Route ("Comments/{ID}")]
        [Authorize]
        public IActionResult DelComments (int ID) {
            GetUserId ();

            var c = diabetes.DeleteComment (ID, user_id);
            if (c)
                return Ok (c);
            else
                return NotFound ();
        }

        //////////////////////////
        ///Get questtion with Doctor answers
        ///while n number of questions needed and id number of the last question
        ///if n =-1 and q  =-1 all question are loaded
        ///
        [HttpGet]
        [Route ("questions/{n}/{id}")]
        [Authorize]
        public IActionResult GetQuestions (int n, int id) {
            GetUserId ();
            // var q = diabetes.GetQuestionsDocAnswers(n, id);

            // var q = diabetes.GetAllQuestions(n, id);

            List<QuestionModel> questions = new List<QuestionModel> ();
            IQueryable<QuestionModel> Q;
            if (id != -1) {
                Q = (from q in db.Questions join u in db.users on q.UserId equals u.UserId where q.QuestionId < id orderby q.Date descending select new QuestionModel {
                    ID = q.QuestionId,
                        UserName = u.UserName,
                        Date = q.Date,
                        Question = q.Question,
                        answers = diabetes.GetDoctorAnswers (q.QuestionId),
                        userid = q.UserId

                });;
            } else {
                Q = (from q in db.Questions join u in db.users on q.UserId equals u.UserId

                    orderby q.Date descending select new QuestionModel {
                        ID = q.QuestionId,
                            UserName = u.UserName,
                            Date = q.Date,
                            Question = q.Question,
                            answers = diabetes.GetDoctorAnswers (q.QuestionId),
                            userid = q.UserId

                    });
            }
            if (n != -1) questions = Q.Take (n).ToList ();
            else questions = Q.ToList ();
            return Ok (questions);
        }

        [HttpGet]
        [Route ("DoctorAns/{id}")]
        [Authorize]
        public IActionResult GetDoctorAns (int id) {
            var A = diabetes.GetDoctorAnswers (id);
            if (A != null)
                return Ok (A);
            return NotFound ();

        }

        [HttpGet]
        [Route ("PatientAns/{id}")]
        [Authorize]
        public IActionResult GetPatientAns (int id) {
            var A = diabetes.GetPatientAnswers (id);
            if (A != null)
                return Ok (A);
            return NotFound ();

        }

        [HttpGet]
        [Route ("AllAns/{id}")]
        [Authorize]
        public IActionResult GetAllAns (int id) {
            var A = diabetes.GetAnswers (id);
            if (A != null)
                return Ok (A);
            return NotFound ();

        }

     

        [HttpPost]
        [Route ("addQuestion/{doctorid}")]
        [Authorize]

        public IActionResult add_Question (int doctorid, [FromBody] string question) {
            GetUserId ();
            if (doctorid == 0) { doctorid = -1; }

            var n = diabetes.AddQuestion (doctorid, user_id, question);
            return Ok (n);
        }

        [HttpGet]
        [Route ("getAllUsers")]
        public IActionResult GetUsers () {
            GetUserId ();
            var n = diabetes.getAllUsers ();
            return Ok (n);
        }

        [HttpGet]
        [Route ("drugs")]
        [Authorize]

        public IActionResult GetDrugs () {
            GetUserId ();
            var n = diabetes.GetAllDrugs (user_id);
            return Ok (n);
        }

        [HttpPost]
        [Route ("savePost/{post_id}")]
        [Authorize]
        public IActionResult add_savedPost (int post_id) {
            UserSavedPosts p = new UserSavedPosts ();
            GetUserId ();
            p.UserId = user_id;
            p.PostId = post_id;

            diabetes.savePost (p);
            return Ok ();
        }

        [HttpGet]
        [Route ("patient/{id}")]
        [Authorize]
        public IActionResult Get_patient (int id) {
            var x = diabetes.getpatientprofile (id);
            return Ok (x);
        }

        [HttpDelete]
        [Route ("Deletepost/{ID}")]
        [Authorize]
        public IActionResult DelPost (int ID) {
            GetUserId ();

            var c = diabetes.Deletepost (ID, user_id);
            if (c)
                return Ok (c);
            else
                return NotFound ();
        }

        [HttpDelete]
        [Route ("DeleteAnswer/{ID}")]
        [Authorize]
        public IActionResult DelAnswer (int ID) {
            GetUserId ();

            var c = diabetes.DeleteAnswer (ID, user_id);
            if (c)
                return Ok (c);
            else
                return NotFound ();
        }

        [HttpDelete]
        [Route ("Deletequestion/{ID}")]
        [Authorize]
        public IActionResult Delquestion (int ID) {
            GetUserId ();

            var c = diabetes.Deletequestion (ID, user_id);
            if (c)
                return Ok (c);
            else
                return NotFound ();
        }

      

       

       

        [HttpGet]
        [Route ("Getuserid")]
        [Authorize]
        public int GetPatientID () {
            GetUserId ();
            return user_id;
        }

        [HttpPost]
        [Route ("Updatedoctorpass")]
        [Authorize]
        public IActionResult Updatedoctorpass ([FromBody] passModel pass) {
            GetUserId ();
            var user = HttpContext.User;
            var u = manager.GetUserAsync (user).Result;
            if (pass.oldPassword != null && pass.newPassword != null) {
                var x = manager.ChangePasswordAsync (u, pass.oldPassword, pass.newPassword).Result;
                return Ok (x);
            }
            return BadRequest ();
        }

        [HttpPost]
        [Route ("ispasstrue")]
        [Authorize]
        public async Task<bool> Ispasstrue ([FromBody] string pass) {
            var User = HttpContext.User;
            var user = await manager.GetUserAsync (User);
            var result = await _signInManager.CheckPasswordSignInAsync (user, pass, false);
            if (result.Succeeded) {
                return true;
            }
            return false;

        }
    }
}