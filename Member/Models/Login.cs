using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Member.Models
{
    public class Login
    {

        [Key]
        [Display(Name = "User ID")]
        public int userId { get; set; }

        
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "email is required.")]
        public string email {get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "password is required.")]
        public string password { get; set; }
        

    }
}