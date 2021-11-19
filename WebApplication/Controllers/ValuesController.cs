using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers {
    [Route ("[controller]")]
    [Authorize (Roles = "Doctor")]
    public class ValuesController : Controller {

        private readonly UserManager<ApplicationUser> _manager;
        public ValuesController (UserManager<ApplicationUser> manager) {
            _manager = manager;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get () {

            return new string[] { _manager.GetUserId (HttpContext.User), _manager.GetUserName (HttpContext.User), "ggggggggggg" };
        }

        // GET api/<controller>/5
        [HttpGet ("{id}")]
        public string Get (int id) {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post ([FromBody] string value) { }

        // PUT api/<controller>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value) { }

        // DELETE api/<controller>/5
        [HttpDelete ("{id}")]
        public void Delete (int id) { }
    }
}