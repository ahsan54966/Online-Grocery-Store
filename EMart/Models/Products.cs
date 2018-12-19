using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMart.Models
{
    public class Products : Category
    {
      

        public int ProductId { get; set; }


        [MinLength(4)]
        [MaxLength(30)]
        [Required(ErrorMessage = "Field Required!")]
        [RegularExpression(@"^[a-zA-Z,!? ]*$", ErrorMessage = "Only Alphabets,Space and comma Allow")]
        public string ProductName { get; set; }



        [Required(ErrorMessage = "Enter Maintainance Expences!")]
        [RegularExpression(@"^[1-9][0-9]{1,5}$", ErrorMessage = "Only Numbers Allowed with 6 length")]

        public int? ProductPrice { get; set; }



        [MinLength(4)]
        [MaxLength(30)]
        [RegularExpression(@"^[a-zA-Z,!? ]*$", ErrorMessage = "Only Alphabets,Space and comma Allow")]
        [Required(ErrorMessage = "Selec Builty Destination !")]
        public string ProductQuality { get; set; }



        public string ProductImage { get; set; }


        [Required(ErrorMessage = "Upload Image")]
        public HttpPostedFileBase image_file { get; set; }

        public int ProductQuantaty { get; set; }
    }
}