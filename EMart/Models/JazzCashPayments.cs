using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMart.Models
{
    public class JazzCashPayments
    {
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        public string RemainingBalance { get; set; }

        public bool AccountIsActive { get; set; }
    }
}