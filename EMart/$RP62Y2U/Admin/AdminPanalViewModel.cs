using EMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMart.ViewModel.Admin
{
    public class AdminPanalViewModel
    {
        public Products product { get; set; }
       
        public List<Category> ShowAllCategory { get; set; }
        public List<Products> ShowAllProduct { get; set; }
    }
}