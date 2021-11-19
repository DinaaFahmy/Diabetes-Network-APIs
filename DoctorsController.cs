using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApplication.Models;
using WebApplication.ModelViews;
using WebApplication.Repo;

namespace WebApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private int user_id;
        private UserManager<ApplicationUser> manager;
        private IDiabetes d;
        public TestController(UserManager<ApplicationUser> _manager, IDiabetes diabetes)
        {
            manager = _manager;
            d = diabetes;

        }

        private void GetUserId()
        {
            user_id = d.GetUserID(manager.GetUserId(HttpContext.User));
        }



        ////////////// doctor profile for patient ////////////////////////////////////

        [HttpGet("{doctorID}")]
        [Route("GetDoctorInfo/{doctorID}")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetDoctorInfo(int doctorID)
        {
            // GetUserId();
            DoctorInfoModel doctorinfo = new DoctorInfoModel();
            Users u = d.GetUser(doctorID);

            doctorinfo.PhoneNumber = manager.Users.Where(a => a.Id == u.ID).Select(a => a.PhoneNumber).FirstOrDefault();



            doctorinfo.UserName = u.UserName;
            doctorinfo.ImageSource = u.ImageSource;

            Doctor doctor = d.GetDoctor(doctorID);

            doctorinfo.Address = doctor.Address;
            doctorinfo.ValidationStatus = doctor.ValidationStatus;

            return Ok(doctorinfo);
        }


        [HttpGet("{doctorID}")]
        [Route("GetDoctorPosts/{doctorID}")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetDoctorPosts(int doctorID)
        {
            //GetUserId();
            List<SavedPosts> doctorposts = new List<SavedPosts>();
            List<Posts> posts = d.GetPostsByUserID(doctorID);

            List<PostModel> postM = new List<PostModel>();
            List<Reactions> reactions = d.GetAllReacts();
            List<Category> categories = d.GetAllCategories();

            for (int i = 0; i < posts.Count; i++)
            {
                PostModel p = new PostModel();
                p.CategoryName = categories.FirstOrDefault(a => a.CategoryId == posts[i].CategoryId).CategoryName;
                p.ReactionName = reactions.FirstOrDefault(a => a.ReactionId == posts[i].ReactionId).ReactionName;
                p.img = posts[i].ImageSource;
                p.date = posts[i].PostDate;
                p.UserName = d.GetUser(posts[i].UserId).UserName;
                p.content = posts[i].PostContent;

                postM.Add(p);
            }

            return Ok(postM);
        }

        [HttpGet("{doctorID}")]
        [Route("GetDoctorQuestions/{doctorID}")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetDoctorQuestions(int doctorID)
        {
            // GetUserId();
            List<QuestionModel> doctorquestions = new List<QuestionModel>();

            List<Questions> questions = d.GetQuestionsByUser(doctorID);
            Users u = d.GetUser(doctorID);

            for (int i = 0; i < questions.Count; i++)
            {
                QuestionModel q = new QuestionModel();

                q.UserName = u.UserName;
                q.ID = questions[i].QuestionId;
                q.Date = questions[i].Date;
                q.Question = questions[i].Question;
                q.answers = d.GetAnswers(questions[i].QuestionId);
                doctorquestions.Add(q);

            }

            return Ok(doctorquestions);

        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetuserQuestions")]
        [Authorize]
        public IActionResult GetuserQuestions()
        {
            GetUserId();
            List<QuestionModel> UserQuestions = new List<QuestionModel>();

            List<Questions> questions = d.GetQuestionsByUser(user_id);
            Users u = d.GetUser(user_id);

            for (int i = 0; i < questions.Count; i++)
            {
                QuestionModel q = new QuestionModel();
                q.ID = questions[i].QuestionId;
                q.UserName = u.UserName;
                q.Date = questions[i].Date;
                q.Question = questions[i].Question;
                q.answers = d.GetAnswers(questions[i].QuestionId);
                UserQuestions.Add(q);
            }


            return Ok(UserQuestions);
        }


        [HttpPost]
        [Route("WritePost")]
        [Authorize]
        public IActionResult WritePost(PostModel post)
        {
            GetUserId();
            List<Reactions> reactions = d.GetAllReacts();
            List<Category> categories = d.GetAllCategories();
            Posts post1 = new Posts();

            post1.UserId = user_id;
            post1.PostContent = post.content;
            post1.PostDate = post.date;
            post1.ImageSource = post.img;
            post1.CategoryId = categories.FirstOrDefault(c => c.CategoryName == post.CategoryName).CategoryId;
            post1.ReactionId = reactions.FirstOrDefault(c => c.ReactionName == post.ReactionName).ReactionId;

            var x =d.AddPost(post1);

            return Ok(x);

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------




        //-http://localhost:51273/api/main/loadposts/-1?ctg=15&ctg=6&ctg=10
        //-http://localhost:51273/api/main/loadposts/20?ctg=15&ctg=17&ctg=11

        [HttpGet("{postID}")]
        [Route("loadPosts/{postID}")] //20
                                      // [Authorize]
        public IActionResult load_ten_posts(int postID, [FromBody] List<int> CategoryId)
        {
           List<Posts> posts  = d.GetPosts(postID, CategoryId);
            List<PostModel> postM = new List<PostModel>();
            List<Reactions> reactions = d.GetAllReacts();
            List<Category> categories = d.GetAllCategories();

            for (int i =0;i<posts.Count; i++)
            {
                PostModel p = new PostModel();
                p.CategoryName = categories.FirstOrDefault(a => a.CategoryId == posts[i].CategoryId).CategoryName;
                p.ReactionName = reactions.FirstOrDefault(a => a.ReactionId == posts[i].ReactionId).ReactionName;
                p.img = posts[i].ImageSource;
                p.date = posts[i].PostDate;
                p.UserName = d.GetUser(posts[i].UserId).UserName;
                p.content = posts[i].PostContent;

                postM.Add(p);
            }

            return Ok(postM);
        }


        [HttpGet]
        [Route("myPosts")]
        [Authorize]
        public IActionResult getUserPosts()
        {
            GetUserId();
            List<Posts> posts = d.GetPostsByUserID(user_id);
            List<PostModel> postM = new List<PostModel>();
            List<Reactions> reactions = d.GetAllReacts();
            List<Category> categories = d.GetAllCategories();

            for (int i = 0; i < posts.Count; i++)
            {
                PostModel p = new PostModel();
                p.CategoryName = categories.FirstOrDefault(a => a.CategoryId == posts[i].CategoryId).CategoryName;
                p.ReactionName = reactions.FirstOrDefault(a => a.ReactionId == posts[i].ReactionId).ReactionName;
                p.img = posts[i].ImageSource;
                p.date = posts[i].PostDate;
                p.UserName = d.GetUser(posts[i].UserId).UserName;
                p.content = posts[i].PostContent;

                postM.Add(p);
            }

            return Ok(postM);
        }


        //IN REVIEW
        //1
        [HttpGet]
        [Route("myDoctors")]
        [Authorize(Roles = "Patient")]//1
        public IActionResult dr()
        {
            GetUserId();
            var n = d.GetMyDoctors(user_id);
            return Ok(n);
        }

        //7 get dr questions he was mentioned in
        [HttpGet]
        [Route("MentionedQuestions")]
        [Authorize(Roles = "Doctor")]
        public IActionResult getMentionedQues()
        {
            GetUserId();
            List<QuestionModel> UserQuestions = new List<QuestionModel>();

            List<Questions> questions = d.getMentionedInQuestions(user_id);

            for (int i = 0; i < questions.Count; i++)
            {
                QuestionModel q = new QuestionModel();
                q.ID = questions[i].QuestionId;
                q.UserName = d.GetUser(questions[i].UserId).UserName;
                q.Date = questions[i].Date;
                q.Question = questions[i].Question;
                q.answers = d.GetAnswers(questions[i].QuestionId);
                UserQuestions.Add(q);
            }

            return Ok(UserQuestions);
        }
            

        //8 add answer -- in review
        [HttpPost]
        [Route("Answer/{questionid}")]
        [Authorize]
        public IActionResult addAnswer( int questionid, [FromBody] string answer)
        {
            GetUserId();

            var n = d.AddAnswer(user_id, questionid, answer);
            return Ok(n);
        }

        //9 get answered questions --in review
        [HttpGet]
        [Route("ansques")]
        [Authorize(Roles = "Doctor")]
        public IActionResult getAnsweredQues()
        {
            GetUserId();
            List<Questions> questions  = d.getAnsweredQuestions(user_id);
            List<QuestionModel> UserQuestions = new List<QuestionModel>();
            Users u = d.GetUser(user_id);

            for (int i = 0; i < questions.Count; i++)
            {
                QuestionModel q = new QuestionModel();
                q.ID = questions[i].QuestionId;
                q.UserName = u.UserName;
                q.Date = questions[i].Date;
                q.Question = questions[i].Question;
                q.answers = d.GetAnswers(questions[i].QuestionId);
                UserQuestions.Add(q);
            }
            return Ok(UserQuestions);
        }


    }
}

