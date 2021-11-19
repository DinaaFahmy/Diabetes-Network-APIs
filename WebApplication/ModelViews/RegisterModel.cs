using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ModelViews
{
    public class RegisterAsPatientModel
    {
        [StringLength(256),Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(256), Required]
        public string UserName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public short MedicalCondetion { get; set; }
        [Required]
        public short Weight { get; set; }
        [Required]
        public short Height { get; set; }
        [Required]
        public short LifeStyle { get; set; }

    }
}
