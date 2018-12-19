using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMart.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string OrderAddress { get; set; }

        public int GrandTotal { get; set; }

        public int OrderCustomerId { get; set; }

        public int OrderShopingId { get; set; }

        public string PaymentStatus { get; set; }

        public string OrderPaymentType { get; set; }

        public string OrderStatus { get; set; }
    }
}