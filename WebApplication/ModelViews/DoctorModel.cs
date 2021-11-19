using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ModelViews
{
    public class DoctorModel
    {

        public  List<Certificates> Certificates { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string UserName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string ImageSource { get; set; }

        public bool ValidationStatus { get; set; }
    }
}
