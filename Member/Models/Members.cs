using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Web.Mvc;
using System.ComponentModel;

namespace Member.Models
{
    public class Members
    {
        [Key]
        [Display(Name = "ID")]
        public int memberId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Member Name is required.")]
        public string fullName { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required.")]
        public string address { get; set; }

        [Display(Name = "Contact")]
        
        public string mobileNumber { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Display(Name = "Gender")]
        public string gender { get; set; }

        [Display(Name = "Member Type")] //Foreign key MemberType.cs
        [Required(ErrorMessage = "Member Type is required.")]
        [ForeignKey("MemberType")]
        public int memberTypeId { get; set; }

        [Display(Name = "Type")]
        public string memberTypeValue { get; set; }
        
      

        [Display(Name = "Entry")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
       

        //[DataType(DataType.Date)]
        public DateTime entryDate { get; set; }


        [Display(Name = "Expiry")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]


        //[DataType(DataType.Date)]
        public DateTime expiryDate { get; set; }


        [Display(Name = "Fee")]
        public int? memberFee { get; set; }

        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Display(Name = "Attachment")]
        public string attachment { get; set; }
        public HttpPostedFileBase attachmentFile { get; set; }

        [Display(Name = "Active")]
       
        public Boolean isActive { get; set; }


        [Display(Name = "Image")]
        public string image { get; set; }

        public HttpPostedFileBase imageFile { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Others
    }
}
