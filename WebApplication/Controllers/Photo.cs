using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers {
    [Route ("[controller]")]
    [ApiController]
    public class Photo : ControllerBase {
        [HttpPost ("UploadFile")]
        public IActionResult UploadFile (IFormFile file) {

            return Ok ();
        }
    }
}