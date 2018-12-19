using EMart.Models;
using EMart.ViewModel.Admin;
using EMart.ViewModel.Customers;
using EMart.ViewModel.Customers.Payments;
using EMart.ViewModel.ShopManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMart.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult AddToCart(string productId = "")
        {
            if (Session["Customer"] == null)
            {
                return Json(new
                {
                    redirectUrl = Url.Action("Login", "Registeration", new { id = 2 }),
                    isRedirect = true
                });
            }

            //here increament value in cart
            //Session["CartOrder"] = Convert.ToInt32(Session["CartOrder"]) + 1; ;\
            else
            {
                var data = new CustomerCRUDViewModel();

                data.AddToCart(productId, Session["CustomerId"].ToString());

                if (Session["CustomerId"] != null)
                {
                    var CartOrderQuantaty = data.CarTOrderQuantaty(Session["CustomerId"]);
                    Session["CartOrderQuantaty"] = CartOrderQuantaty;
                }
                else
                {

                    Session["CartOrderQuantaty"] = 0;
                }




                return Json("Product Add", JsonRequestBehavior.AllowGet);
            }
        }

        /*
         
         
        [AllowAnonymous]
        public ActionResult checkOut()
        {
            if (Session["Customer"] == null)
            {
                return RedirectToAction("Login", "Registeration", new { id = 2 });
            }
            var data = new AdminPanalCRUDViewModel();

            var product = data.CheckOut(Session["CustomerId"]);


            return View(product);

        }
        */
        public ActionResult checkOut()
        {
            if (Session["Customer"] == null)
            {
                return RedirectToAction("Login", "Registeration", new { id = 2 });
            }
            var data = new CustomerCRUDViewModel();

            var product = data.CheckOut(Session["CustomerId"].ToString());
            return View(product);

        }



        [HttpPost]
        public JsonResult ButtonAction(string Action, string ProductId, string CustomerId)
        {
            var CustomerModel = new CustomerCRUDViewModel();

            CustomerModel.QuantatyUpdate(Action, ProductId, CustomerId);


            return Json("Quantaty Add", JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult YesProceedToCheckOut(string UserAddress = "", string CustomerId = "", string GrandTotal = "", string OrderPaymentType="")
        {
            var data = new CustomerCRUDViewModel();

            var product = data.YesProceedToCheckOut(UserAddress, CustomerId, GrandTotal,OrderPaymentType);
            return Json(new
            {
                redirectUrl = Url.Action("Index", "Home"),
                isRedirect = true
            });
        
        }

        public ActionResult CustomerClaims()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CustomerClaims(string claim="")
        {
            var method = new CustomerCRUDViewModel();
            string message = "";
            int CustomerId = int.Parse(Session["CustomerId"].ToString());
            try
            {
                TempData["ClaimHomeMessage"] = method.Claim(claim, CustomerId);
            }
            catch (Exception e)
            {
                TempData["ClaimHomeMessage"] = e.Message;
            }
            return View();
        }

        public ActionResult CustomerPayments(int id=0)
        {
            var paymentlist = new List<Payment>();
            var method = new OrderCrudViewModel();
            try
            {
                paymentlist = method.GetPaymentsDetail(id,"Customer");
                if (paymentlist.Count == 0)
                {
                    TempData["PaymentHomeMessage"] = "No Payment History Yet Now";
                }
            }
            catch (Exception e)
            {
                TempData["PaymentHomeMessage"] = e.Message;
            }
            var viewmodel = new PaymentViewModel();
            viewmodel.PaymentList = paymentlist;
            return View(viewmodel);
        }
        public JsonResult GetPaymentDetail(string paymentid="")
        {
            int PaymentId = int.Parse(paymentid);
            var method = new CustomerCRUDViewModel();
            Payment PaymentData = method.GetPaymentDetail(PaymentId);
            return Json(PaymentData,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CustomerFinishPayment(string paymentid="")
        {
            int PaymentId = int.Parse(paymentid);
            var method = new CustomerCRUDViewModel();
            string message = "";
            try
            {
                message = method.FinalPayment(PaymentId);
            }
            catch(Exception e)
            {
                TempData["CustomerPayment"] = e.Message;
            }
            return RedirectToAction("CustomerPayments",new { id = Session["CustomerId"]});
        }
        public ActionResult OrderHistory(int id=0)
        {
            var orderlist = new List<Order>();
            var method = new OrderCrudViewModel();
            try
            {
                orderlist = method.GetOrderHistory(id, "CustomerHistory");
                if (orderlist.Count == 0)
                {
                    TempData["OrderHistoryMessage"] = "No Order History Yet Now";
                }
            }
            catch (Exception e)
            {
                TempData["OrderHistoryMessage"] = e.Message;
            }

            return View(orderlist);

        }

        public ActionResult OrderHistoryDetail(int id = 0, int userid = 0)
        {
            var orderview = new List<OrderDetailView>();
            var method = new OrderCrudViewModel();
            try
            {
                orderview = method.OrderHistoryDetail(id, userid, "CustomerHistory");
            }
            catch (Exception e)
            {
                TempData["OrderHistoryMessage"] = e.Message;
            }
            return View(orderview);
        }

        public ActionResult Action()
        {
            return View();
        }

    }
}