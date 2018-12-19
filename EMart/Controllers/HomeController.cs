using EMart.ViewModel.Admin;
using EMart.ViewModel.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMart.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index(int CategoryId = 1)
        {
            var viewmodel = new AdminPanalViewModel();

            var data = new CustomerCRUDViewModel();

            var category = data.ShowAllCatogery();
            var product = data.ShowAllProduct(CategoryId);

            viewmodel.ShowAllCategory = category;    // here is a list of show all product in view model

            viewmodel.ShowAllProduct = product;

            if (Session["CustomerId"] != null)
            {
                var CartOrderQuantaty = data.CarTOrderQuantaty(Session["CustomerId"]);
                Session["CartOrderQuantaty"] = CartOrderQuantaty;
            }
            else
            {

                Session["CartOrderQuantaty"] = 0;
            }
            return View(viewmodel);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}