using EMart.ViewModel.ShopManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMart.Models;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using EMart.ViewModel.Registeration;
namespace EMart.Controllers
{
    public class ShopManagerController : Controller
    {
        // GET: ShopManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Order()
        {
            var orderlist = new List<Order>();
            var method = new OrderCrudViewModel();
            try
            {
                orderlist = method.GetAllOrders();
                if (orderlist.Count==0)
                {
                    TempData["OrderHomeMessage"]="No Order Yet Now";
                }
            }
            catch(Exception e)
            {
                TempData["OrderHomeMessage"] = e.Message;
            }

            return View(orderlist);
        }

        public ActionResult OrderDetail(int id=0)
        {
            var orderview = new List<OrderDetailView>();
            var method = new OrderCrudViewModel();
            try
            {
                orderview = method.GetOrderDetail(id);
            }
            catch (Exception e)
            {
                TempData["OrderHomeMessage"] = e.Message;
            }
            return View(orderview);
        }


        public ActionResult OrderConfirm(int id = 0, int ShopId = 0,int CustomerId=0,int Total=0,string PaymentType="")
        {
//            //bool redirect = false;
            var OrderData=new DataSet();
            var method = new OrderCrudViewModel();
            try
            {
                OrderData = method.ConfirmOrder(id,ShopId,CustomerId,Total,PaymentType);
            }
            catch (Exception e)
            {
                TempData["OrderHomeMessage"] = e.Message;
            }
            if(OrderData.Tables.Count>0)
            {
                TempData["BillingData"] = OrderData;
                return RedirectToAction("GenerateBill");
            }

            var orderview = new List<OrderDetailView>();

            try
            {
                orderview = method.GetOrderDetail(id);
            }
            catch (Exception e)
            {
                TempData["OrderHomeMessage"] = e.Message;
            }
            return View("Order",orderview);
            //return Json(new { redirectUrl = Url.Action("GenerateBill", "ShopManger", new { id = 2 }), isRedirect = redirect }, JsonRequestBehavior.AllowGet);
        }

        //Generate Bills
        public void GenerateBill()
        {
            var ReportData = (DataSet)TempData["BillingData"];
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report/StoreBill.rpt")));
            rd.SetDataSource(ReportData);
            Response.Clear();
            string filepath = Server.MapPath("~/" + "Bill.pdf");
            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filepath);
            FileInfo fileinfo = new FileInfo(filepath);
            Response.AddHeader("Content-Disposition", "inline;filenam=StoreBill.pdf");
            Response.ContentType = "application/pdf";
            Response.WriteFile(fileinfo.FullName);
        }


        // Here is Live Order
        public ActionResult LiveOrder(int id=0)
        {
            var orderlist = new List<Order>();
            var method = new OrderCrudViewModel();
            try
            {
                orderlist = method.GetLiveOrder(id);
                if (orderlist.Count == 0)
                {
                    TempData["OrderLiveMessage"] = "No Order Yet Now";
                }
            }
            catch (Exception e)
            {
                TempData["OrderLiveMessage"] = e.Message;
            }

            return View(orderlist);
        }

        public ActionResult LiveOrderDetail(int id = 0, int userid=0)
        {
            var orderview = new List<OrderDetailView>();
            var method = new OrderCrudViewModel();
            try
            {
                orderview = method.GetLiveOrderDetail(id,userid);
            }
            catch (Exception e)
            {
                TempData["OrderLiveMessage"] = e.Message;
            }
            return View(orderview);
        }
        // For Completion of Order
        public ActionResult OrderCompletion(int id=0)
        {
            
            if(id!=0)
            {
                var method = new OrderCrudViewModel();
                try
                {
                    TempData["OrderLiveMessage"] = method.OrderCompletion(id);
                }
                catch (Exception e)
                {
                    TempData["OrderLiveMessage"] = e.Message;
                }
            }
            return RedirectToAction("LiveOrder", new { id = Session["ShopManagerId"] });
        }


        // Order History

        public ActionResult OrderHistory(int id=0)
        {
                
                var orderlist = new List<Order>();
                var method = new OrderCrudViewModel();
                try
                {
                    orderlist = method.GetOrderHistory(id,"History");
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
        public ActionResult OrderHistoryDetail(int id=0,int userid=0)
        {
            var orderview = new List<OrderDetailView>();
            var method = new OrderCrudViewModel();
            try
            {
                orderview = method.OrderHistoryDetail(id, userid,"History");
            }
            catch (Exception e)
            {
                TempData["OrderHistoryMessage"] = e.Message;
            }
            return View(orderview);
        }


        //Payments
        public ActionResult Payments(int id=0)
        {
            var paymentlist = new List<Payment>();
            var method = new OrderCrudViewModel();
            try
            {
                paymentlist = method.GetPaymentsDetail(id,"ShopManager");
                if (paymentlist.Count == 0)
                {
                    TempData["PaymentHomeMessage"] = "No Payment History Yet Now";
                }
            }
            catch (Exception e)
            {
                TempData["PaymentHomeMessage"] = e.Message;
            }

            return View(paymentlist);
        }

        public ActionResult ProfileSetting(int id = 0)
        {
            var method = new RegisterationCrudViewModel();
            User edituser = method.EditUser(id);
            return View(edituser);
        }
        [HttpPost]
        public ActionResult ProfileSetting(User user)
        {
            var method = new RegisterationCrudViewModel();

            string message = "";
            if (ModelState.IsValidField("FirstName") && ModelState.IsValidField("LastName") && ModelState.IsValidField("UserAddress") && ModelState.IsValidField("UserPassword") && ModelState.IsValidField("ConfirmPassword"))
            {
                try
                {
                    message = method.UpdateUser(user);


                    if (message == "1")
                    {
                        message = "Shop Updated Successfully";
                        TempData["UserUpdateInfo"] = message;
                    }
                    else if (message == "0")
                    {
                        message = "Shop Not Updated";
                        TempData["UserUpdateInfo"] = message;
                    }
                }
                catch (Exception e)
                {
                    TempData["UserUpdateInfo"] = e.Message;
                }
            }

            return View(user);
        }
    }
}