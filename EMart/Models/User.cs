using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMart.Models
{
    public class User
    {
        
        public int UserId { get; set; }

    
        [Required(ErrorMessage = "Please Enter First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter First Name In Correct Form")]
        [MinLength(1)]
        [MaxLength(25)]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Please Enter Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter First Name In Correct Form")]
        [MinLength(1)]
        [MaxLength(25)]
        public string LastName { get; set; }

        

        [Required(ErrorMessage="Please Enter Email")]
        [MinLength(10, ErrorMessage = "Enter Valid Email Length")]
        [MaxLength(40, ErrorMessage = "Enter Valid Email Length")]
        [RegularExpression(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$", ErrorMessage = "Enter Correct Email Address")]
        public string UserEmail { get; set; }




        [Required(ErrorMessage = "Please Enter Password")]
        [RegularExpression(@"^^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,14}$", ErrorMessage = "Password should contain one capital letter and a number atleast")]
        [MinLength(6)]
        [MaxLength(14)]
        public string UserPassword { get; set; }


        [Required(ErrorMessage = "Please Enter Password Again")]
        [Compare("UserPassword",ErrorMessage = "Password not Matched")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Please Enter Phone Number")]
        [MinLength(11, ErrorMessage = "Enter Valid Phone Number Length")]
        [MaxLength(11, ErrorMessage = "Enter Valid Phone Number Length")]
        [RegularExpression(@"^03[0-9]{9}$", ErrorMessage = "Enter Valid Phone Number")]
        public string PhoneNumber { get; set; }


        public string UserLat { get; set; }

        public string UserLon { get; set; }


        [Required(ErrorMessage = "Please Enter Addess")]
        [MinLength(3)]
        [MaxLength(50)]
        public string UserAddress { get; set; }

        public string IsActive { get; set; }

        public int UserRoleId { get; set; }

        public string UserImageUrl { get; set; }


        [Required(ErrorMessage = "Upload Image")]
        public HttpPostedFileBase UserImage { get; set; }


        [Required(ErrorMessage = "Please Enter Verfiy Code")]
        public string VerifyCode { get; set; }
    }
}