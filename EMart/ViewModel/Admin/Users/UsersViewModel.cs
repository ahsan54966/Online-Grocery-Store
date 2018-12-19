using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMart.Models;
namespace EMart.ViewModel.Admin.Users
{
    public class UsersViewModel
    {
        public User Users { get; set; }

        public ShopManager ShopManagers { get; set; }

        public Customer Customers { get; set; }

        public Role Roles { get; set; }
    }
}