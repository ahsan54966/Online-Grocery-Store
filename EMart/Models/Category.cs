using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMart.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [MinLength(4)]
        [MaxLength(30)]
        [Required(ErrorMessage = "Field Required!")]
        [RegularExpression(@"^[a-zA-Z,!? ]*$", ErrorMessage = "Only Alphabets,Space and comma Allow")]
        public string CategoryName { get; set; }


        public string CatogoryImage_Path { get; set; }

        [Required(ErrorMessage = "Upload Image")]
        public HttpPostedFileBase Categoryimage_file { get; set; }
    }
}