using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMart.Models
{
    public class RegisterationViewModel
    {
        public User User { get; set; }

        public ShopManager ShopManager { get; set; }

        public Customer Customer { get; set; }

        public Admin Admin { get; set; }
    }
}