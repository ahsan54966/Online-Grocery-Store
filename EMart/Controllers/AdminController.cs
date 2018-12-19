using EMart.App_Start;
using EMart.Models;
using EMart.ViewModel;
using EMart.ViewModel.Admin;
using EMart.ViewModel.Admin.Users;
using EMart.ViewModel.Customers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMart.Controllers
{

    public class AdminController : Controller
    {
            [AdminAuthentication]
        // GET: Admin
        public ActionResult Index()
        {
            var userlist = new List<UsersViewModel>();
            var method = new UsersCrudViewModel();
            try
            {
                userlist = method.GetAllUser();
                if (userlist.Count == 0)
                {
                    TempData["AdminHomeMessage"] = "No Users Register Yet Now";
                }
            }
            catch (Exception e)
            {
                TempData["AdminHomeMessage"] = e.Message;
            }

            return View(userlist);
        }
            public ActionResult EnableOrDisableUser(int id=0,string IsActive="")
            {
                var method = new UsersCrudViewModel();
                try
                {
                    TempData["AdminHomeMessage"] = method.EnableOrDisableUser(id,IsActive);
                    
                }
                catch (Exception e)
                {
                    TempData["AdminHomeMessage"] = e.Message;
                }

                return RedirectToAction("Index");
            }

        /*
        public ActionResult E_Mart_Home(int CategoryId = 1)
        {
            var viewmodel = new AdminPanalViewModel();

            var data = new AdminPanalCRUDViewModel();

            var category = data.ShowAllCatogery();
            var product = data.ShowAllProduct(CategoryId);

            viewmodel.ShowAllCategory = category;    // here is a list of show all product in view model

            viewmodel.ShowAllProduct = product;

            Session["UserId"] = 1019;
            return View(viewmodel);

        }
  
         */

        
            [AdminAuthentication]
        //Admin panal for add the item
        public ActionResult AdminPanalHome()
        {
            var data = new AdminPanalCRUDViewModel();

            var products = data.ShowAllProduct();
            var viewmodel = new AdminPanalViewModel();
            viewmodel.ShowAllProduct = products;    // here is a list of show all product in view model

            return View(viewmodel);

        }
            [HttpPost]
            public JsonResult AllCategory()
            {

                  var data = new CustomerCRUDViewModel();

            var category = data.ShowAllCatogery();

                return Json(category, JsonRequestBehavior.AllowGet);

            }

        [AdminAuthentication]
        [HttpPost]
        public ActionResult Addproduct(Products product)
        {
            //if (ModelState.IsValid)
            //{
                var data = new AdminPanalCRUDViewModel();

                var Products = PictureSave(product); //here picture methos calll

                data.AddProduct(Products);
            //}

            return RedirectToAction("AdminPanalHome", "Admin");
        }


        [AdminAuthentication]
        [HttpPost]
        public ActionResult AddCategory(Products product)
        {
                var data = new AdminPanalCRUDViewModel();

                var Products = PictureSave(product); //here picture methos calll

                data.AddCategory(Products);
            

            return RedirectToAction("AdminPanalHome", "Admin");
        }




            [AdminAuthentication]
        [HttpPost]
        public JsonResult EditProduct(string eid, string pid)
        {

            int id = int.Parse(eid);
            var data = new AdminPanalCRUDViewModel();
            var FormData = data.EditProduct(id);
            // var viewmodel = new AdminPanalViewModel();
            //viewmodel.product = FormData;




            return Json(FormData, JsonRequestBehavior.AllowGet);

        }

            [AdminAuthentication]

        public JsonResult UpdateProduct(Products product)
        {
            //ModelState["image_file"].Errors.Clear();
            //if (ModelState.IsValid)
            //{

                var data = new AdminPanalCRUDViewModel();

                if (product.image_file != null)
                {
                    Products products = PictureSave(product);
                    data.UpdateProduct(product);
                    return Json("update product", JsonRequestBehavior.AllowGet);
                }

                if (product.image_file == null)
                {

                    data.UpdateProduct(product);
                    return Json("update product", JsonRequestBehavior.AllowGet);
                }

            //}
            return Json("check validation", JsonRequestBehavior.AllowGet);
        }






            [AdminAuthentication]
        public Models.Products PictureSave(Products product)
        {
            if (product.image_file.FileName != "")
            {
                string filename = Path.GetFileNameWithoutExtension(product.image_file.FileName);
                string extension = Path.GetExtension(product.image_file.FileName).ToLower();
                if (extension == ".jpg" || extension == ".png") //check file type
                {
                    filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    product.ProductImage = "~/Pictures/ProductPicture/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Pictures/ProductPicture/"), filename);
                    product.image_file.SaveAs(filename);
                    TempData["UserData"] = product;
                }
                else
                {
                    TempData["extensionerror"] = "Plz check Your Image Type";

                }
            }

            return product;

        }


        /// Here is User Management Section


        
    }
}
