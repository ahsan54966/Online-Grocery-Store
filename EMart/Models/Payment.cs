using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMart.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int PaymentOrderId { get; set; }

        public int PaymentTotal { get; set; }

        public string PaymentMethod { get; set; }

        public int CustomerId { get; set; }

        public int ShopId { get; set; }

        public string PaymentStatus { get; set; }
    }
}