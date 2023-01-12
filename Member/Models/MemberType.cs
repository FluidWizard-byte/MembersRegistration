using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Member.Models
{
    public class MemberType
    {
        [Key]
        public int memberTypeId { get; set; }

        [Display(Name = "Member Type")]
        [Required(ErrorMessage = "Member Type is required.")]
        public string type { get; set; }
    }
}