using EMart.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace EMart.ViewModel.Registeration
{
    public class RegisterationCrudViewModel
    {
                //here is user verified user valid or not
        public string register(User Reg)
        {
            string message = "";
            string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;
            
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("validuser", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;


                    cm.Parameters.AddWithValue("@PhoneNumber", Reg.PhoneNumber);
                    cm.Parameters.AddWithValue("@UserEmail", Reg.UserEmail);



                    SqlDataReader dr = cm.ExecuteReader();
                    if (dr.Read())
                    {
                        //Reg.error = "Sorry this user is already exist";
                        message = "0";
                    }
                    else
                    {

                        message = "1";

                        //     con.Close();

                    }


                }
                return message;
            }

        }

        //here after verify user save in database also forgin key store
        public int Admin_Register(RegisterationViewModel verifiedUser)
        {
            //encrypted password
            string enc = verifiedUser.Admin.UserPassword;
            byte[] encode = new byte[enc.Length];
            encode = Encoding.UTF8.GetBytes(verifiedUser.Admin.UserPassword);
            string enc_password = Convert.ToBase64String(encode);


            int user_id=0;
            string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connection))
            {


                con.Open();
                using (SqlCommand cmd = new SqlCommand("Admin_Registration", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserFirstName", verifiedUser.Admin.FirstName);
                    cmd.Parameters.AddWithValue("@UserLastName", verifiedUser.Admin.LastName);
                    cmd.Parameters.AddWithValue("@UserEmail", verifiedUser.Admin.UserEmail);
                    cmd.Parameters.AddWithValue("@UserPassword", enc_password);
                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@UserRoleId", 1);
                    cmd.Parameters.AddWithValue("@UserImageUrl", verifiedUser.Admin.UserAddress);
                    cmd.Parameters.AddWithValue("@UserPhoneNumber", verifiedUser.Admin.PhoneNumber);
                    cmd.Parameters.AddWithValue("@UserAddress", verifiedUser.Admin.UserImageUrl);
                    cmd.Parameters.AddWithValue("@UserLat", verifiedUser.Admin.UserLat);
                    cmd.Parameters.AddWithValue("@UserLon", verifiedUser.Admin.UserLon);


                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if(dt.Rows.Count>0)
                    {
                        user_id = int.Parse(dt.Rows[0]["UserId"].ToString());
                    }
                    
                    /*
                 SqlDataReader dr = cmd.ExecuteReader();
                 if(dr.HasRows)
                 {
                     if (dr.Read())
                     {
                         verifiedUser.User.UserId = int.Parse(dr["UserId"].ToString());
                     }
                 }

                 
                     
                 //company detail
                 con.Close();
                 con.Open();
                 using (SqlCommand cm = new SqlCommand("Company_info", con))
                 {
                     cm.CommandType = System.Data.CommandType.StoredProcedure;

                     cm.Parameters.AddWithValue("@CompanyRegNum", verifiedUser.Comp_Reg_No);
                     cm.Parameters.AddWithValue("@CompanyOwner", verifiedUser.Comp__owner_name);
                     //cm.Parameters.AddWithValue("@owner_cnic", verifiedUser.owner_cnic);
                     //cm.Parameters.AddWithValue("@owner_ph", verifiedUser.owner_ph);
                     cm.Parameters.AddWithValue("@CompanyVehicleQuanitity", verifiedUser.comp_no_vehicle);
                     cm.Parameters.AddWithValue("@CompanyWorth", verifiedUser.comp_worth);
                     cm.Parameters.AddWithValue("@CompanyAddress", verifiedUser.comp_adress);
                     cm.Parameters.AddWithValue("@CompanyPhoneNumber", verifiedUser.CompanyPhoneNumber);
                     cm.Parameters.AddWithValue("@CompanyUserId", verifiedUser.UserId);
                     cm.ExecuteNonQuery();
                     con.Close();
                  * */
                      
                    
                }

            }
            return user_id;
        }

        //here after verify customer save in database also forgin key store
        public string Customer_register(RegisterationViewModel verifiedUser)
        {
            //encrypted password
            string enc = verifiedUser.Customer.UserPassword;
            byte[] encode = new byte[enc.Length];
            encode = Encoding.UTF8.GetBytes(verifiedUser.Customer.UserPassword);
            string enc_password = Convert.ToBase64String(encode);

            string message = "";
            string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connection))
            {


                con.Open();
                using (SqlCommand cmd = new SqlCommand("User_Registration", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserFirstName", verifiedUser.Customer.FirstName);
                    cmd.Parameters.AddWithValue("@UserLastName", verifiedUser.Customer.LastName);
                    cmd.Parameters.AddWithValue("@UserEmail", verifiedUser.Customer.UserEmail);
                    cmd.Parameters.AddWithValue("@UserPassword", enc_password);
                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@UserRoleId", 2);
                    cmd.Parameters.AddWithValue("@UserImageUrl", verifiedUser.Customer.UserAddress);
                    cmd.Parameters.AddWithValue("@UserPhoneNumber", verifiedUser.Customer.PhoneNumber);
                    cmd.Parameters.AddWithValue("@UserAddress", verifiedUser.Customer.UserImageUrl);
                    cmd.Parameters.AddWithValue("@UserLat", verifiedUser.Customer.UserLat);
                    cmd.Parameters.AddWithValue("@UserLon", verifiedUser.Customer.UserLon);
                    if(cmd.ExecuteNonQuery()>0)
                    {
                        message = "User sucessfully add";
                    }
                    else
                    {
                        throw new Exception("Problem During Registeration");
                    }
                    con.Close();
                }
                return message;
            }
        }

        //here is forget password uwer exist or not it valid user check with email
        public User forgetpassword(User Reg)
        {

            string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("forgetpassword", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;



                    cm.Parameters.AddWithValue("@UserEmail", Reg.UserEmail);


                    SqlDataReader dr = cm.ExecuteReader();
                    if (dr.Read())
                    {

                        Reg.FirstName = dr["UserFirstName"].ToString();
                        Reg.LastName = dr["UserLastName"].ToString();
                        Reg.PhoneNumber = dr["UserPhoneNumber"].ToString();

                    }
                    //else
                    //{
                    //}
                }


            }
            return Reg;
        }

        //here user reset password  
        public string UpdatePassword(User reg, string user_ph)
        {
            //encrypted password
            string enc = reg.UserPassword;
            byte[] encode = new byte[enc.Length];
            encode = Encoding.UTF8.GetBytes(reg.UserPassword);
            string enc_password = Convert.ToBase64String(encode);

            string message = "";
            string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("UpdatePassword", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;



                    cm.Parameters.AddWithValue("@UserPassword", enc_password);
                    cm.Parameters.AddWithValue("@UserPhoneNumber", user_ph);




                    //SqlDataReader dr = cm.ExecuteReader();
                    if (cm.ExecuteNonQuery() > 0)
                    {
                        message = "1";

                    }
                    con.Close();
                }


                return message;


            }
        }

        public List<User> userlogin(User log, int f_key)
        {
            //encrypted password
            string enc = log.UserPassword;
            byte[] encode = new byte[enc.Length];
            encode = Encoding.UTF8.GetBytes(log.UserPassword);
            string enc_password = Convert.ToBase64String(encode);
            List<User> reglist = new List<User>();
            string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("Login", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;



                    cm.Parameters.AddWithValue("@UserEmail", log.UserEmail);

                    cm.Parameters.AddWithValue("@UserPassword", enc_password);
                    cm.Parameters.AddWithValue("@f_key", f_key);

                    SqlDataReader dr = cm.ExecuteReader();
                    if (dr.Read())
                    {
                        log.FirstName = dr["UserFirstName"].ToString();
                        log.LastName = dr["UserLastName"].ToString();
                        log.UserId = int.Parse(dr["UserId"].ToString());
                        log.UserImageUrl = dr["UserImageUrl"].ToString();
                    }
                    //else
                    //{
                    //}
                }
                reglist.Add(log);
                return reglist;

            }
        }

        /*
public RegisterationViewModel EditUser(int id)
{

 *             User user=new User();
string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

using (SqlConnection con = new SqlConnection(connection))
{
    con.Open();
    using (SqlCommand cm = new SqlCommand("sp_User_EditUser", con))
    {
        cm.CommandType = System.Data.CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@UserId", id);

        SqlDataReader dr = cm.ExecuteReader();
        if (dr.Read())
        {
            user.UserFirstName = dr["User_FirstName"].ToString();
            user.UserLastName = dr["User_LastName"].ToString();
            user.UserAddress = dr["User_Address"].ToString();
            user.User_Password = dr["User_Password"].ToString();
            user.ConfirmPassowrd = dr["User_Password"].ToString();
            user.Image = dr["User_Image"].ToString();
            user.UserId =int.Parse(dr["UserId"].ToString());

        }

        //decrypted password
        string dec = user.User_Password;
        byte[] encodedDataAsBytes = System.Convert.FromBase64String(dec);
        string dec_pass = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
        //else
        //{
        //}
        user.User_Password = dec_pass;
        user.ConfirmPassowrd = dec_pass;
    }
}
    return user;
             

}
         * */
        public string UpdateUser(RegisterationViewModel user)
        {
            //encrypted password
            string enc = user.User.UserPassword;
            byte[] encode = new byte[enc.Length];
            encode = Encoding.UTF8.GetBytes(user.User.UserPassword);
            string enc_password = Convert.ToBase64String(encode);
            string message = "";
            string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("sp_User_UpdateUser", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@User_FirstName", user.User.FirstName);
                    cm.Parameters.AddWithValue("@User_LastName", user.User.LastName);
                    cm.Parameters.AddWithValue("@User_Address", user.User.UserAddress);
                    cm.Parameters.AddWithValue("@UserId", user.User.UserId);
                    cm.Parameters.AddWithValue("@User_Password", enc_password);
                    cm.Parameters.AddWithValue("@User_Image", user.User.UserImageUrl);
                    int check=cm.ExecuteNonQuery();
                    if (check>0)
                    {
                        message = "1";
                    }
                    else if(check==1)
                    {
                        message = "0";
                    }
                    else
                    {
                        throw new Exception("Database Problem");
                    }

                }
            }
            return message;
        }


        public string ShopkeeperRegister(RegisterationViewModel verifiedUser)
        {
            //encrypted password
            string enc = verifiedUser.ShopManager.UserPassword;
            byte[] encode = new byte[enc.Length];
            encode = Encoding.UTF8.GetBytes(verifiedUser.ShopManager.UserPassword);
            string enc_password = Convert.ToBase64String(encode);


            string user_id = "";
            string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connection))
            {


                con.Open();
                using (SqlCommand cmd = new SqlCommand("ShopManagerRegistration", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserFirstName", verifiedUser.ShopManager.FirstName);
                    cmd.Parameters.AddWithValue("@UserLastName", verifiedUser.ShopManager.LastName);
                    cmd.Parameters.AddWithValue("@UserEmail", verifiedUser.ShopManager.UserEmail);
                    cmd.Parameters.AddWithValue("@UserPassword", enc_password);
                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@UserRoleId", 3);
                    cmd.Parameters.AddWithValue("@UserImageUrl", verifiedUser.ShopManager.UserAddress);
                    cmd.Parameters.AddWithValue("@UserPhoneNumber", verifiedUser.ShopManager.PhoneNumber);
                    cmd.Parameters.AddWithValue("@UserAddress", verifiedUser.ShopManager.UserImageUrl);
                    cmd.Parameters.AddWithValue("@UserLat", verifiedUser.ShopManager.UserLat);
                    cmd.Parameters.AddWithValue("@UserLon", verifiedUser.ShopManager.UserLon);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        user_id = dr["UserId"].ToString();
                    }
                    /*
                     
                    //company detail
                    con.Close();
                    con.Open();
                    using (SqlCommand cm = new SqlCommand("Company_info", con))
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;

                        cm.Parameters.AddWithValue("@CompanyRegNum", verifiedUser.Comp_Reg_No);
                        cm.Parameters.AddWithValue("@CompanyOwner", verifiedUser.Comp__owner_name);
                        //cm.Parameters.AddWithValue("@owner_cnic", verifiedUser.owner_cnic);
                        //cm.Parameters.AddWithValue("@owner_ph", verifiedUser.owner_ph);
                        cm.Parameters.AddWithValue("@CompanyVehicleQuanitity", verifiedUser.comp_no_vehicle);
                        cm.Parameters.AddWithValue("@CompanyWorth", verifiedUser.comp_worth);
                        cm.Parameters.AddWithValue("@CompanyAddress", verifiedUser.comp_adress);
                        cm.Parameters.AddWithValue("@CompanyPhoneNumber", verifiedUser.CompanyPhoneNumber);
                        cm.Parameters.AddWithValue("@CompanyUserId", verifiedUser.UserId);
                        cm.ExecuteNonQuery();
                        con.Close();
                     * */
                    return "";

                }
            }
        }
    }
}