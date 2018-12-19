using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMart.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }
    }
}