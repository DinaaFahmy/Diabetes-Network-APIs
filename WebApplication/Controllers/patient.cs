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
        [Route("myDoctors")]
        [Authorize(Roles = "Patient")] //1
        public IActionResult dr()
        {
            GetUserId();
            var n = d.GetMyDoctors(user_id);
            return Ok(n);
        }


    }




    //[Route("[controller]")]

    //[Authorize]
    public partial class restController : ControllerBase
    {
        [HttpGet]
        [Route("GetPatientPoints")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetPatientPoints()
        {
            GetUserId();
            Patient p = diabetes.GetPatient(user_id);
            short points;
            if (p.Points != null)
            {
                points = p.Points.Value;
            }
            else
            {
                points = 0;
            }
            return Ok(points);
        }

        [HttpGet]
        [Route("MedicalInfo")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetMedicalInfo()
        {
            GetUserId();
            return Ok(diabetes.GetPatient(user_id));
        }

        [HttpPost]
        [Route("MedicalInfo")]
        [Authorize(Roles = "Patient")]
        public IActionResult UpdateMedicalInfo([FromBody] Patient patient)
        {
            GetUserId();
            diabetes.UpdatePatient(patient);
            return Ok(patient);
        }

        [HttpGet]
        [Route("A1C")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetLastA1C()
        {
            GetUserId();
            short A1Ctype = 0;
            diabetes.GetAllTests(user_id, A1Ctype);
            return Ok(diabetes.GetAllTests(user_id, A1Ctype).LastOrDefault());
        }

        [HttpPost]
        [Route("Test")]
        [Authorize(Roles = "Patient")]
        public IActionResult AddTest([FromBody] Test test)
        {
            GetUserId();
            test.PatientId = user_id;
            //   string ID =User.Claims.First(i => i.Type == "UserId").Value;
            var s = diabetes.AddTest(test);
            return Ok(s);
        }

        [HttpGet]
        [Route("LastCheckup")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetLastCheckup()
        {
            GetUserId();
            return Ok(diabetes.GetPatientCheckups(user_id).LastOrDefault());
        }

        [HttpGet]
        [Route("Checkups")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetAllCheckups()
        {
            GetUserId();
            return Ok(diabetes.GetPatientCheckups(user_id));
        }

        [HttpPost]
        [Route("Checkups")]
        [Authorize(Roles = "Patient")]
        public IActionResult AddCheckup([FromBody] ChecksUps checksUp)
        {
            GetUserId();
            checksUp.PatientId = user_id;

            var x = diabetes.AddCheckup(checksUp);
            return Ok(x);
        }

        [HttpPut]
        [Route("Checkups")]
        [Authorize(Roles = "Patient")]
        public IActionResult UpdateCheckup([FromBody] ChecksUps checksUp)
        {
            GetUserId();
            if (checksUp.PatientId == user_id)
            {
                return Ok(diabetes.UpdateCheckup(checksUp));
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("DeleteCheckups")]
        [Authorize(Roles = "Patient")]
        public IActionResult DeleteCheckup([FromBody] ChecksUps checksUp)
        {
            GetUserId();
            if (checksUp.PatientId == user_id)
            {
                return Ok(diabetes.DeleteCheckup(checksUp));
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("Test")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetTests()
        {
            GetUserId();
            return Ok(diabetes.GetAllTests(user_id));
        }

        [Route("react")]
        public IActionResult getReacts()
        {
            var c = diabetes.GetAllReacts();
            return Ok(c);
        }

        [HttpGet("{doctorid}")]
        [Route("unfollowdr/{doctorid}")]
        [Authorize(Roles = "Patient")]
        public IActionResult unfollow_dr(int doctorid) //11 / 14
        {
            GetUserId();

            var n = diabetes.UnfollowDoctor(user_id, doctorid);
            return Ok(n);
        }

        [HttpPost("{followID}/{status}")]
        [Route("access_medinfo/{status}")] //2 / 0
        [Authorize(Roles = "Patient")]
        ////////////////////////////////may need modification
        public IActionResult update_access_to_medInfo([FromBody] int followID, short status)
        {
            //status = 0 rejected 2 approved
            var n = diabetes.Update_access_medicalInfo(followID, status);
            return Ok(n);
        }
        [HttpGet]
        [Route("MyprofilePatient")]
        [Authorize(Roles = "Patient")]
        public IActionResult GetMyProfilePatient()
        {
            GetUserId();
            PatientFullModel p = diabetes.GetPatientProfile(user_id);
            p.email = manager.GetUserAsync(HttpContext.User).Result.Email;
            p.phone_number = manager.GetUserAsync(HttpContext.User).Result.PhoneNumber;

            return Ok(p);

        }

        [HttpPut]
        [Route("MyprofilePatient")]
        [Authorize(Roles = "Patient")]
        public IActionResult UpdateMyProfilePatient([FromBody] PatientFullModel patient)
        {
            GetUserId();
            var user = HttpContext.User;
            var u = manager.GetUserAsync(user).Result;
            if (patient.password != null && patient.newpassword != null)
            {
                var x = manager.ChangePasswordAsync(u, patient.password, patient.newpassword).Result;
            }
            var t = manager.ChangePhoneNumberAsync(u, patient.phone_number, manager.GenerateChangePhoneNumberTokenAsync(u, patient.phone_number).Result).Result;
            var p = diabetes.UpdatePatientProfile(user_id, patient);

            p.email = manager.GetUserAsync(HttpContext.User).Result.Email;
            p.phone_number = manager.GetUserAsync(HttpContext.User).Result.PhoneNumber;

            return Ok(p);

        }

        [HttpPost("{doctorid}")]
        [Route("followdr/{doctorid}/{access_med_info}")]
        [Authorize(Roles = "Patient")]

        public IActionResult follow_dr(int doctorid, short access_med_info)
        {
            GetUserId();

            var n = diabetes.FollowDoctor(user_id, doctorid, access_med_info);
            return Ok(n);
        }

        [HttpGet]
        [Route("getmytests/{type}")]
        [Authorize(Roles = "Patient")]

        public IActionResult Gettests(short type)
        {
            GetUserId();
            var n = diabetes.GetAllTests(user_id, type);
            return Ok(n);
        }

        [HttpGet]
        [Route("getAlltests")]
        [Authorize(Roles = "Patient")]

        public IActionResult Gettestfordoctor(int patientid)
        {
            GetUserId();
            var n = diabetes.getAllTestswithouttype(user_id);
            return Ok(n);
        }

        [HttpPost]
        [Route("Deletetest")]
        [Authorize(Roles = "Patient")]
        public IActionResult Deltest([FromBody] Test t)
        {
            GetUserId();

            db.Test.Remove(t);
            db.SaveChanges();
            return Ok();

        }

        [HttpGet]
        [Route("isfollowhim/{Doctorid}")]

        [Authorize(Roles = "Patient")]
        public bool Isfollow(int Doctorid)
        {
            GetUserId();
            List<FollowingPatients> n = diabetes.GetMyPatients(Doctorid);

            if (n.FirstOrDefault(a => a.userID == user_id) != null)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

    }






}