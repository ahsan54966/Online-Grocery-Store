using EMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMart.ViewModel.ShopManagers
{
    public class OrderDetailView
    {
        public Products Product { get; set; }

        public Order Order { get; set; }

        public OrderDetail OrderDetail { get; set; }
    }
}