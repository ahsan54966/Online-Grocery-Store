using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMart.Models;
namespace EMart.ViewModel.Customers.Payments
{
    public class PaymentViewModel
    {
        public List<Payment> PaymentList { get; set; }
        public Payment Payments { get; set; }
    }
}