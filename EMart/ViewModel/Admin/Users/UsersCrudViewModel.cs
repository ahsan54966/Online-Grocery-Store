using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMart.ViewModel.Admin.Users;
using EMart.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace EMart.ViewModel.Admin.Users
{
    public class UsersCrudViewModel
    {
        string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;
        public List<UsersViewModel> GetAllUser()
        {
            List<UsersViewModel> UserList = new List<UsersViewModel>();
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_User_GetAllUsers", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    var users = new UsersViewModel();
                    User user = new User();
                    user.UserId = int.Parse(sdr["UserId"].ToString());
                    user.UserEmail = sdr["UserEmail"].ToString();
                    //user.UserShopingId = int.Parse(sdr["UserShopingId"].ToString());
                    user.IsActive = sdr["IsActive"].ToString();
                    user.PhoneNumber = sdr["UserPhoneNumber"].ToString();
                    user.UserAddress = sdr["UserAddress"].ToString();
                    user.FirstName = sdr["UserFirstName"].ToString();
                    user.LastName = sdr["UserLastName"].ToString();
                    Role role=new Role();
                    role.RoleId=int.Parse(sdr["RoleId"].ToString());
                    role.RoleName = sdr["RoleName"].ToString();
                    users.Users = user;
                    users.Roles = role;
                    UserList.Add(users);
                }
            }
            con.Close();
            return UserList;
        }


        // Disable or Enable User

        public string EnableOrDisableUser(int id=0,string IsActive="")
        {
            string message = "";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_User_EnableOrDisableUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId",id);
            cmd.Parameters.AddWithValue("@IsActive",IsActive);
            if (cmd.ExecuteNonQuery()>0)
            {
                if(IsActive=="1")
                {
                    message = "User Activated";
                }
                else if(IsActive=="0")
                {
                    message = "User Disabled";
                }

            }
            con.Close();
            return message;
        }
    }
}