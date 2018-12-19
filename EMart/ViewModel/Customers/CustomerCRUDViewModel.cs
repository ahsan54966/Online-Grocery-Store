using EMart.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EMart.ViewModel.Customers
{
   
        public class CustomerCRUDViewModel
        {

            string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;



            public List<Category> ShowAllCatogery()
            {
                List<Category> category = new List<Category>();
                // string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand cmd = new SqlCommand("AllCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Category cat = new Category();
                        cat.CategoryId = int.Parse(sdr["CategoryId"].ToString());
                        cat.CategoryName = sdr["CategoryName"].ToString();

                        cat.CatogoryImage_Path = sdr["CatogoryImage_Path"].ToString();



                        category.Add(cat);
                    }


                }

                return category;
            }

            public List<Products> ShowAllProduct(int CategoryId)
            {
                List<Products> product = new List<Products>();
                // string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand cmd = new SqlCommand("AllProductShow", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CategoryId", CategoryId);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Products pro = new Products();
                        pro.ProductId = int.Parse(sdr["ProductId"].ToString());
                        pro.ProductName = sdr["ProductName"].ToString();
                        pro.ProductPrice = int.Parse(sdr["ProductPrice"].ToString());
                        pro.ProductImage = sdr["ProductImage"].ToString();
                        pro.ProductQuality = sdr["ProductQuality"].ToString();

                        product.Add(pro);
                    }


                }

                return product;
            }


            public void QuantatyUpdate(string Action, string ProductId, string CustomerId)
            {
                SqlConnection con = new SqlConnection(connection);

                con.Open();
                SqlCommand cmd0 = new SqlCommand("AddToCart_UpdateQuantaty", con);
                cmd0.CommandType = CommandType.StoredProcedure;
                cmd0.Parameters.AddWithValue("@productId", ProductId);
                cmd0.Parameters.AddWithValue("@CustomerId", CustomerId);
                cmd0.Parameters.AddWithValue("Action", Action);
                cmd0.ExecuteNonQuery();
                con.Close();


            }


            public void AddToCart(string productId, string CustomerId)
            {
                SqlConnection con = new SqlConnection(connection);
                con.Open();

                SqlCommand cmd = new SqlCommand("AddToCart_ProductExist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    con.Close();
                    con.Open();
                    SqlCommand cmd0 = new SqlCommand("AddToCart_UpdateQuantaty", con);
                    cmd0.CommandType = CommandType.StoredProcedure;
                    cmd0.Parameters.AddWithValue("@productId", productId);
                    cmd0.Parameters.AddWithValue("@CustomerId", CustomerId);
                    cmd0.Parameters.AddWithValue("@Action", "Add");
                    cmd0.ExecuteNonQuery();


                }
                else
                {
                    con.Close();
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("AddToCart", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@ProductId", productId);
                    cmd1.Parameters.AddWithValue("@CustomerId", CustomerId);

                    cmd1.ExecuteNonQuery();
                    con.Close();
                }
            }

            public int CarTOrderQuantaty(object p)
            {
                int CartOrderQuantaty = 0;
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand cmd = new SqlCommand("CartOrderQuantaty", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CustomerId", p);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {

                        int CartQuantaty = int.Parse(sdr["ProductQuantity"].ToString());
                        CartOrderQuantaty += CartQuantaty;
                    }
                }
                return CartOrderQuantaty;
            }
            public List<Products> CheckOut(string p)
            {

                List<Products> product = new List<Products>();
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand cmd = new SqlCommand("CheckOut", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CustomerId", p);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Products pro = new Products();
                        pro.ProductId = int.Parse(sdr["ProductId"].ToString());
                        pro.ProductName = sdr["ProductName"].ToString();
                        pro.ProductPrice = int.Parse(sdr["ProductPrice"].ToString());
                        pro.ProductImage = sdr["ProductImage"].ToString();
                        pro.ProductQuality = sdr["ProductQuality"].ToString();

                        pro.ProductQuantaty = int.Parse(sdr["ProductQuantity"].ToString());

                        product.Add(pro);
                    }


                }

                return product;
            }






            public string YesProceedToCheckOut(string UserAddress, string CustomerId, string GrandTotal,string OrderPaymentType="")
            {
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                string Message="";
                SqlCommand cmd = new SqlCommand("FinalCheckOut", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UserAddress", UserAddress);
                cmd.Parameters.AddWithValue("CustomerId", CustomerId);
              
                cmd.Parameters.AddWithValue("GrandTotal", GrandTotal);
                cmd.Parameters.AddWithValue("OrderPaymentType", OrderPaymentType);
                if(cmd.ExecuteNonQuery()>1)
                {
                    Message = "ok";
                }
                con.Close();
                return Message;
            }

            public Payment GetPaymentDetail(int PaymentId=0)
            {
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                Payment payment = new Payment();
                SqlCommand cmd = new SqlCommand("Sp_Payment_EditPayment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaymentId", PaymentId);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {

                    if (sdr.Read())
                    {

                        payment.PaymentId = int.Parse(sdr["PaymentId"].ToString());
                        payment.PaymentOrderId = int.Parse(sdr["PaymentOrderId"].ToString());
                        payment.PaymentTotal = int.Parse(sdr["PaymentTotal"].ToString());
                        payment.PaymentMethod = sdr["PaymentMethod"].ToString();
                        payment.CustomerId = int.Parse(sdr["CustomerId"].ToString());
                        payment.ShopId = int.Parse(sdr["ShopId"].ToString());
                        payment.PaymentStatus = sdr["PaymentStatus"].ToString();

                    }


                }


                con.Close();
                return (payment);
            }

            public string FinalPayment(int PaymentId = 0)
            {
                string message = "";
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                Payment payment = new Payment();
                SqlCommand cmd = new SqlCommand("Sp_Payment_FinalPayment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaymentId", PaymentId);
                if (cmd.ExecuteNonQuery()>0)
                {
                    message = "1";
                }


                con.Close();
                return message;
            }

            public string Claim(string claim = "", int CustomerID = 0)
            {
                
                string message = "";
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                Payment payment = new Payment();
                SqlCommand cmd = new SqlCommand("Sp_Claim_DoClaim", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerId", CustomerID);
                cmd.Parameters.AddWithValue("@Claim", claim);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    message = "Your Have Been Claimed Successfully. We will Resolve your Problems Soon";
                }


                con.Close();
                return message;
            }

        }
    
}