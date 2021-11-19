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
using System;
using Newtonsoft.Json.Linq;


namespace WebApplication.Controllers
{
    //[Route("[controller]")]

    public partial class TestController : ControllerBase
    {
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
                q.userid = questions[i].UserId;
                UserQuestions.Add(q);
            }

            return Ok(UserQuestions);
        }

        [HttpGet]
        [Route("ansques")]
        [Authorize(Roles = "Doctor")]
        public IActionResult getAnsweredQues()
        {
            GetUserId();
            List<Questions> questions = d.getAnsweredQuestions(user_id);
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
                q.userid = questions[i].UserId;
                UserQuestions.Add(q);
            }
            return Ok(UserQuestions);
        }

    }

    //[Route("[controller]")]

    [Authorize]
    public partial class restController : ControllerBase
    {
        [HttpGet]
        [Route("patientCheckups/{id}")]
        [Authorize(Roles = "Doctor")]

        public IActionResult GetPatientCheckups(int id)
        {
            GetUserId();

            List<FollowingPatients> n = diabetes.GetMyPatients(user_id);

            if (n.FirstOrDefault(a => a.userID == id) != null)
            {
                var acess = db.PatientDoctorsFollow.ToList().FirstOrDefault(a => a.PatienId == id && a.DoctorId == user_id).AccessMedicalInfo;
                if (acess == 2)
                {
                    var nn = diabetes.GetPatientCheckups(id);
                    return Ok(nn);
                }
                else
                {
                    return BadRequest("لا تملك الصلاحية لرؤية هذه المعلومات");
                }
            }

            return BadRequest("لا تملك الصلاحية لرؤية هذه المعلومات");

        }

        [HttpGet]
        [Route("doctor")]
        [Authorize(Roles = "Doctor")]
        public IActionResult getDrInfo()
        {
            GetUserId();
            DoctorModel doctorinfo = new DoctorModel();
            Users u = diabetes.GetUser(user_id);

            doctorinfo.PhoneNumber = manager.Users.Where(a => a.Id == u.ID).Select(a => a.PhoneNumber).FirstOrDefault();

            doctorinfo.UserName = u.UserName;
            doctorinfo.ImageSource = u.ImageSource;
            doctorinfo.Email = manager.Users.Where(a => a.Id == u.ID).Select(a => a.Email).FirstOrDefault();
            doctorinfo.Password = manager.Users.Where(a => a.Id == u.ID).Select(a => a.PasswordHash).FirstOrDefault();
            Doctor doctor = diabetes.GetDoctor(user_id);
            doctorinfo.Certificates = diabetes.GetCertificates(user_id);
            doctorinfo.Address = doctor.Address;
            doctorinfo.ValidationStatus = doctor.ValidationStatus;

            return Ok(doctorinfo);
        }

        //2 update dr info
        //[HttpPut]
        //[Route("drupdate")]
        //public IActionResult updateDrInfo(Doctor doctor)
        //{
        //    //Users u = d.GetUser(doctor.DoctorId);
        //    var n = diabetes.UpdateDoctor(doctor);
        //    return Ok(n);
        //}
        //3 get all following patients
        [HttpGet]
        [Route("getpatients")]
        [Authorize(Roles = "Doctor")]
        public IActionResult getAllFollowingPatients()
        {
            GetUserId();
            var n = diabetes.GetMyPatients(user_id);
            return Ok(n);
        }

        //4 request access for medical info
        [HttpPost]
        [Route("request/{pid}")]
        [Authorize(Roles = "Doctor")]
        public IActionResult request_access_medicalInfo(int pid, [FromBody] int FollowID) //gives error in function
        {
            GetUserId();

            var n = diabetes.request_access_medicalInfo(user_id, pid, FollowID);
            //var x = db.PatientDoctorsFollow.Where(a => a.Id == FollowID).FirstOrDefault();
            //var y = db.PatientDoctorsFollow.Find(FollowID);
            if (n != null)

                return Ok(n);
            return BadRequest();
        }

        [HttpPost("{address}")]
        [Route("updateAddress/{address}")]
        [Authorize(Roles = "Doctor")]
        public IActionResult updateAddress([FromBody] string address) //gives error in function
        {
            GetUserId();
            diabetes.updateAddress(user_id, address);
            return Ok();
        }

        [HttpPost]
        [Route("Addecertificate")]
        [Authorize(Roles = "Doctor")]
        public IActionResult Addecertificate(Certificates certificate) //gives error in function
        {
            GetUserId();

            certificate.DoctorId = user_id;
            diabetes.Addecertificate(certificate);
            return Ok();
        }

        [HttpGet]
        [Route("gettests/{patientid}/{type}")]
        [Authorize(Roles = "Doctor")]

        public IActionResult Gettestsfordoctor(int patientid, short type)
        {
            GetUserId();
            List<FollowingPatients> n = diabetes.GetMyPatients(user_id);

            if (n.FirstOrDefault(a => a.userID == patientid) != null)
            {
                var acess = db.PatientDoctorsFollow.ToList().FirstOrDefault(a => a.PatienId == patientid && a.DoctorId == user_id).AccessMedicalInfo;
                if (acess == 2)
                {
                    var nn = diabetes.GetAllTests(patientid, type);
                    return Ok(nn);
                }
                else
                {
                    return BadRequest("لا تملك الصلاحية لرؤية هذه المعلومات");
                }
            }

            return BadRequest("لا تملك الصلاحية لرؤية هذه المعلومات");

        }

        [HttpGet]
        [Route("getAlltests/{patientid}")]
        [Authorize(Roles = "Doctor")]

        public IActionResult Gettestsfordoctor(int patientid)
        {
            GetUserId();
            var n = diabetes.getAllTestswithouttype(patientid);
            return Ok(n);
        }
    }
    }
