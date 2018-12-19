using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMart.Models;
using EMart.ViewModel.Registeration;
using System.IO;
using System.Web.Helpers;
namespace EMart.Controllers
{
    public class RegisterationController : Controller
    {
        // GET: Registeration
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CustomerRegister(int id=0)
        {
            Session["LoginId"] = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerRegister(RegisterationViewModel Reg, string command) //command recive with form button
        {



            ModelState["Customer.VerifyCode"].Errors.Clear();
  

            //sing up for customer Role and Register For Admin And Rgistered for Driver
            if (command == "Sing Up")
            {

                if (ModelState.IsValid)
                {
                    RegisterationCrudViewModel cust_reg = new RegisterationCrudViewModel();

                   

                string userexist= cust_reg.register(Reg.Customer); // chek valid user

       
                   if (userexist=="0") //if user is already register 
                   {

                       TempData["Erroruserexist"] = "User is already exist ,Plz login";
                       return RedirectToAction("Login", "Registeration", new {id=2 });

                   }
                   else
                   {
                       //Picture Save after user verification
                       Reg.Customer.UserImageUrl=PictureSave(Reg.Customer.UserImage);
                       TempData["UserData"] = Reg;
                        //picture method called it define in botton 
                       if (TempData["extensionerror"] == null) //checked extension
                       {
                           TempData["RegistrationRole"] = command;
                           TempData.Keep();
                           codesendmail();                    //mail send method called
                           return RedirectToAction("VerfyCode", "Registeration");
                       }
                       else
                       {
                           return RedirectToAction("CustomerRegister", "Registeration"); //redirect customer page with error message
                       }
                   }
                }


            }
            return View();

        }

       //here is picture method called in customer,admin and driver
        public string PictureSave(HttpPostedFileBase UserImage)
        {
            string UserImageUrl = "";
            if(UserImage.FileName!=null)
            {
                string filename = Path.GetFileNameWithoutExtension(UserImage.FileName);
                string extension = Path.GetExtension(UserImage.FileName).ToLower();
                if (extension == ".jpg" || extension == ".png") //check file type
                {
                    filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    UserImageUrl = "~/Pictures/User/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Pictures/User/"), filename);
                    UserImage.SaveAs(filename);
                }
                else
                {
                    TempData["extensionerror"] = "Plz check Your Image Type";

                }
            }

            return UserImageUrl;
        
        }


        //send mail method are call in customer,driver,admin
        public ActionResult codesendmail()
        {

            string numbers = "0123456789";
            Random objrandom = new Random();
            string strrandom = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                strrandom += temp;
            }
            ViewBag.otp = strrandom;

            TempData["code"] = strrandom;

            TempData.Keep();

            RegisterationViewModel verifiedUser = TempData["Userdata"] as RegisterationViewModel;
            //string ahsan = verifiedUser.UserFirstName;
            if (verifiedUser.Admin != null)
            {
                WebMail.Send(verifiedUser.Admin.UserEmail, "Well Come '" + verifiedUser.Admin.UserEmail + "_" + verifiedUser.Admin.UserEmail + "' Emart", "Dear Sir,<br/> You are Register this mobile NO  :" + verifiedUser.Admin.PhoneNumber + "<br/><br/> Your are Register With Email  :" + verifiedUser.Admin.UserEmail + "<br/><br/>Your Verfy Code Is :" + strrandom
, null, null, null, true, null, null, null, null, null, null);
            }
            else if (verifiedUser.ShopManager != null)
            {
                WebMail.Send(verifiedUser.ShopManager.UserEmail, "Well Come '" + verifiedUser.ShopManager.UserEmail + "_" + verifiedUser.ShopManager.UserEmail + "' Emart", "Dear Sir,<br/> You are Register this mobile NO  :" + verifiedUser.ShopManager.PhoneNumber + "<br/><br/> Your are Register With Email  :" + verifiedUser.ShopManager.UserEmail + "<br/><br/>Your Verfy Code Is :" + strrandom
, null, null, null, true, null, null, null, null, null, null);
            }
            else if (verifiedUser.Customer != null)
            {
                WebMail.Send(verifiedUser.Customer.UserEmail, "Well Come '" + verifiedUser.Customer.UserEmail + "_" + verifiedUser.Customer.UserEmail + "' Emart", "Dear Sir,<br/> You are Register this mobile NO  :" + verifiedUser.Customer.PhoneNumber + "<br/><br/> Your are Register With Email  :" + verifiedUser.Customer.UserEmail + "<br/><br/>Your Verfy Code Is :" + strrandom
, null, null, null, true, null, null, null, null, null, null);
            }

            TempData.Keep();
            return RedirectToAction("VerfyCode", "Registeration");

        }
        //forget Password sends mail

        //send mail method are call in customer,driver,admin
        public ActionResult forgetsendmail()
        {

            string numbers = "0123456789";
            Random objrandom = new Random();
            string strrandom = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                strrandom += temp;
            }
            ViewBag.otp = strrandom;

            TempData["code"] = strrandom;

            TempData.Keep();

            User verifiedUser = TempData["Userdata"] as User;
            //string ahsan = verifiedUser.UserFirstName;


            WebMail.Send(verifiedUser.UserEmail, "Well Come '" + verifiedUser.FirstName + "_" + verifiedUser.LastName + "'  Emart", "Dear Sir,<br/> You are Register this mobile NO  :" + verifiedUser.PhoneNumber + "<br/><br/> Your are Register With Email  :" + verifiedUser.UserEmail + "<br/><br/>Your Verfy Code Is :" + strrandom
            , null, null, null, true, null, null, null, null, null, null);


            TempData.Keep();
            return RedirectToAction("VerfyCode", "Registeration");

        }


          //the page goes to verfy code so here chek the code and submit the form
        public ActionResult FinalyCustomerRegister()
        {

            return View();
        }
        [HttpPost]
        public ActionResult FinalyCustomerRegister(RegisterationViewModel REG, string command1)
        {
          
           //firstly we check a code is npot equall
            string role= (TempData["RegistrationRole"].ToString()); 
            string C;
            if (TempData["code"] != null) //code recive in send mail method
            {
                C = (TempData["code"].ToString());  //code assign in varible c for if check

         
          if (REG.User.VerifyCode==C)                          //code match ,user enter
          {

                RegisterationViewModel verifiedUser = TempData["Userdata"] as RegisterationViewModel;   //tempdata user(user reg form) assign in veifieduser
                RegisterationCrudViewModel cust_reg = new RegisterationCrudViewModel();
                if (role == "Sing Up") //its for customer sing up
              {
                    cust_reg.Customer_register(verifiedUser);

                    //PictureSave(REG);
                    return RedirectToAction("Login", "Registeration", new { id=Session["LoginId"]}); //user sucessfullly registered and goes customer home page
              }

                else if (role == "Register")// its for admin
              {
                  cust_reg.Admin_Register(verifiedUser);
                 // PictureSave(REG);
                  List<RegisterationViewModel> Reg = new List<RegisterationViewModel>();
                  //Reg.Add(verifiedUser);
                  Session["UserData"] = Reg;

                  return RedirectToAction("Login", "Registeration", new { id = Session["LoginId"] }); //user sucessfullly registered and goes  admin home page
              }
                else if(role=="Registers")
                {
                    cust_reg.ShopkeeperRegister(verifiedUser);
                    // PictureSave(REG);
                    List<RegisterationViewModel> Reg = new List<RegisterationViewModel>();
                    //Reg.Add(verifiedUser);
                    Session["UserData"] = Reg;

                    return RedirectToAction("Login", "Registeration", new { id = Session["LoginId"] }); //user sucessfullly registered and goes  admin home page
                }
                else if(role=="Reset Password")
              {
                  return RedirectToAction("UpdatePassword", "Registeration", new { id=Session["LoginId"]}); //user sucessfullly registered and goes update  home page
              }

          }
          else
          {
              TempData["WrongCode"] = "Yor code is not verified <br/> Plz TRY Again"; //code is not verified
             
          }
            }
            else
            {
                TempData["Errormessage"] = "Enter your 5 digit code"; //code is not entered,
            }
            if (command1 == "Resend Code") //here is code is resend code and redirect to verify code page
            {

                string numbers = "0123456789";
                Random objrandom = new Random();
                string strrandom = string.Empty;
                for (int i = 0; i < 5; i++)
                {
                    int temp = objrandom.Next(0, numbers.Length);
                    strrandom += temp;
                }
                ViewBag.otp = strrandom;

                TempData["code"] = strrandom;
                TempData.Keep();
               
                RegisterationViewModel verifiedUser = TempData["Userdata"] as RegisterationViewModel;
                //string ahsan = verifiedUser.UserFirstName;
                if(verifiedUser.Admin!=null)
                {
                    WebMail.Send(verifiedUser.Admin.UserEmail, "Well Come '" + verifiedUser.Admin.UserEmail + "_" + verifiedUser.Admin.UserEmail + "' Emart", "Dear Sir,<br/> You are Register this mobile NO  :" + verifiedUser.Admin.PhoneNumber + "<br/><br/> Your are Register With Email  :" + verifiedUser.Admin.UserEmail + "<br/><br/>Your Verfy Code Is :" + strrandom
, null, null, null, true, null, null, null, null, null, null);
                }
                else if(verifiedUser.ShopManager!=null)
                {
                    WebMail.Send(verifiedUser.ShopManager.UserEmail, "Well Come '" + verifiedUser.ShopManager.UserEmail + "_" + verifiedUser.ShopManager.UserEmail + "' Emart", "Dear Sir,<br/> You are Register this mobile NO  :" + verifiedUser.ShopManager.PhoneNumber + "<br/><br/> Your are Register With Email  :" + verifiedUser.ShopManager.UserEmail + "<br/><br/>Your Verfy Code Is :" + strrandom
, null, null, null, true, null, null, null, null, null, null);
                }
                else if (verifiedUser.Customer!= null)
                {
                    WebMail.Send(verifiedUser.Customer.UserEmail, "Well Come '" + verifiedUser.Customer.UserEmail + "_" + verifiedUser.Customer.UserEmail + "' Emart", "Dear Sir,<br/> You are Register this mobile NO  :" + verifiedUser.Customer.PhoneNumber + "<br/><br/> Your are Register With Email  :" + verifiedUser.Customer.UserEmail + "<br/><br/>Your Verfy Code Is :" + strrandom
, null, null, null, true, null, null, null, null, null, null);
                }




                TempData.Keep();
                TempData["ResendMessage"] = "Check Your Email You get 5 digit Verify Code"; //code is not entered,
                return RedirectToAction("VerfyCode", "Registeration", new { id = Session["LoginId"] });

            }
          return RedirectToAction("VerfyCode", "Registeration");
        }


        //admin registration like as customer and driver

        public ActionResult AdminRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminRegister(RegisterationViewModel Reg,string command)
        {
            if (command == "Register")
            {
                //ModelState["PinCode"].Errors.Clear();
                //ModelState["VehicleNumber"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    RegisterationCrudViewModel Admin = new RegisterationCrudViewModel();

                   

                string userexist= Admin.register(Reg.Admin);

       
                   if (userexist=="0")
                   {
                       TempData["Erroruserexist"] = "User is already exist ,Plz login";
                       return RedirectToAction("AdminRegister", "Registeration");
                   }
                   else
                   {
                       Reg.Admin.UserImageUrl = PictureSave(Reg.Admin.UserImage);
                       TempData["UserData"] = Reg;
                       if (TempData["extensionerror"] == null)
                       {
                           TempData["RegistrationRole"] = command;
                           TempData.Keep();
                           codesendmail();
                            return RedirectToAction("VerfyCode", "Registeration");
                       }

                       else
                       {
                           return RedirectToAction("AdminRegister", "Registeration");
                       } 
                   }
                }


            }
            return View();
        }

        //// Here is Shop Keeper Registeration
        //admin registration like as customer and driver

        public ActionResult ShopManagerRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopManagerRegister(RegisterationViewModel Reg, string command)
        {
            if (command == "Registers")
            {
                //ModelState["PinCode"].Errors.Clear();
                //ModelState["VehicleNumber"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    RegisterationCrudViewModel ShopManager = new RegisterationCrudViewModel();



                    string userexist = ShopManager.register(Reg.ShopManager);


                    if (userexist == "0")
                    {
                        TempData["Erroruserexist"] = "User is already exist ,Plz login";
                        return RedirectToAction("ShopManagerRegister", "Registeration");
                    }
                    else
                    {
                        Reg.ShopManager.UserImageUrl = PictureSave(Reg.ShopManager.UserImage);
                        TempData["UserData"] = Reg;
                        if (TempData["extensionerror"] == null)
                        {
                            TempData["RegistrationRole"] = command;
                            TempData.Keep();
                            codesendmail();
                            return RedirectToAction("VerfyCode", "Registeration");
                        }

                        else
                        {
                            return RedirectToAction("ShopManagerRegister", "Registration");
                        }
                    }
                }


            }
            return View();
        }





        //here is start login we get id as from button
        public ActionResult Login(int id=0)
        {
            Session["LoginId"] = id;
              return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User log, int id)
        {
//            
            if (ModelState.IsValidField("UserEmail") && ModelState.IsValidField("UserPassword"))
            {
                string username = "";
                int f_key = id; //here we get forgin key and 

                RegisterationCrudViewModel login = new RegisterationCrudViewModel();

             
                    //TempData["Userdata"] = login.userlogin(log, f_key);
                    var user = login.userlogin(log, f_key);
                    TempData.Keep();
//                  
                






                if (user.FirstName != null)
                {
                    //Session["loginuser"] = userdata.user_first_name;


                    //if (Session["loginuser"] != "")
                    //{
                    //here cheked id we recive in button and redirect with specfic id
                    if (id == 1)
                    {
                        
                        Session["AdminData"] = user;
//                        Session["AdminId"] = user.UserId;
                        Session["user_Id"] = user.UserId;
                        return RedirectToAction("Index", "Admin");

                    }
                    if (id == 2)
                    {
                        Session["Customer"] = user;
                        Session["CustomerId"] = user.UserId;

                        return RedirectToAction("Index", "Home");

                    }
                    if (id == 3)
                    { //its goes to ShopKeeper index page
                        //Session["user"] =userdata.user_first_name;
                        Session["ShopManager"] = user;
                        Session["ShopManagerId"] = user.UserId;

                        return RedirectToAction("Index", "ShopManager");

                    }
                }
                else
                {
                    TempData["message"] = "Login failed Try Again";
                }


            }

                return View();


           
        }
        //its page form action is finalregistration method
        public ActionResult VerfyCode()
        {

            //if (Session["user"] == null)
            //{
            //    return RedirectToAction("Login");
            //}

            return View();
        }
        //firstly check user is exist then alow to reset password
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(User Reg,string command)
        {
            if (ModelState.IsValidField("UserEmail"))
            {
                RegisterationCrudViewModel cust_reg = new RegisterationCrudViewModel();
                if (command == "Reset Password")
                {
                    TempData["UserData"] = cust_reg.forgetpassword(Reg);


                    User CheckedUser = TempData["Userdata"] as User;
                    string phone = CheckedUser.PhoneNumber;
                    TempData["Phone"] = phone;
                    if (phone == null) //dont get valu data base show error message
                    {

                        TempData["Error"] = "User is Not exist Plz Try Again";
                        return RedirectToAction("ForgetPassword", "Registeration");

                    }
                    else //if user is valid then send code and redirect in verify code
                    {
                        TempData["RegistrationRole"] = command;
                        TempData.Keep();
                        forgetsendmail();
                        return RedirectToAction("VerfyCode", "Registeration", new { id = Session["LoginId"] });   
                    }
                }
            }
            return View();
        }

        //here is update password and goes to login
        public ActionResult UpdatePassword()
        {
            TempData.Keep();
            return View();
        }
    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(User Reg,string user_ph)
        {

            user_ph= (TempData["Phone"].ToString()); 
            RegisterationCrudViewModel cust_reg = new RegisterationCrudViewModel();
            string check=cust_reg.UpdatePassword(Reg,user_ph);
            if(check=="1")
            {
                TempData["updatemessage"] = "Password Updated Successfully";
            }
            else
            {
                TempData["updatemessage"] = "Problem";
            }

            return RedirectToAction("Login", "Registeration", new { id=Session["LoginId"]});   
        }
    public ActionResult Logout(int id=0)
    {
        Session.RemoveAll();
        return RedirectToAction("Index", "Home");
            
    }
    }
}