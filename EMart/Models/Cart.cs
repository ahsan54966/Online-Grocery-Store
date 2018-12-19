using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMart.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public int CartProductId { get; set; }

        public int CartCustomerId { get; set; }
    }
}