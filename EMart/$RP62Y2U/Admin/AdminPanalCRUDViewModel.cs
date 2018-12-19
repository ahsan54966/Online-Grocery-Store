using EMart.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EMart.ViewModel.Admin
{
    public class AdminPanalCRUDViewModel
    {
        string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;
        public List<Products> ShowAllProduct()
        {
            List<Products> product = new List<Products>();
           // string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("AllProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
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
                    pro.CategoryName =sdr["CategoryName"].ToString();


                    product.Add(pro);
                }
            
            
            }

            return product;
        }

        public void AddProduct(Products Products)
        {


            List<Products> product = new List<Products>();
            //string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;

            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Category_CategoryExist", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("CategoryName", Products.CategoryName);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                if (sdr.Read())
                {
                    int CategoryId = int.Parse(sdr["CategoryId"].ToString());
                    con.Close();
                    con.Open();
                    SqlCommand cmd0 = new SqlCommand("sp_AddProduct", con);
                    cmd0.CommandType = CommandType.StoredProcedure;
                    cmd0.Parameters.AddWithValue("ProductName", Products.ProductName);
                    cmd0.Parameters.AddWithValue("ProductPrice", Products.ProductPrice);
                    cmd0.Parameters.AddWithValue("ProductQuality", Products.ProductQuality);
                    cmd0.Parameters.AddWithValue("ProductImage", Products.ProductImage);
                    cmd0.Parameters.AddWithValue("CategoryId", CategoryId);
                    cmd0.ExecuteNonQuery();
                    con.Close();
                    cmd.Dispose();
                }


            }

            else
            {
                
                con.Close();
                con.Open();
                SqlCommand cmd1 = new SqlCommand("Sp_AddCategory", con);
                
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("CategoryName", Products.CategoryName);
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                if (sdr1.HasRows)
                {
                    if (sdr1.Read())
                    {

                        int CategoryId = int.Parse(sdr1["CategoryId"].ToString());
                        con.Close();
                        con.Open();
                        SqlCommand cmd0 = new SqlCommand("sp_AddProduct", con);
                        cmd0.CommandType = CommandType.StoredProcedure;
                        cmd0.Parameters.AddWithValue("ProductName", Products.ProductName);
                        cmd0.Parameters.AddWithValue("ProductPrice", Products.ProductPrice);
                        cmd0.Parameters.AddWithValue("ProductQuality", Products.ProductQuality);
                        cmd0.Parameters.AddWithValue("ProductImage", Products.ProductImage);
                        cmd0.Parameters.AddWithValue("CategoryId", CategoryId);
                        cmd0.ExecuteNonQuery();
                        con.Close();
                    
                    }                
                
                
                }

            
            }


        }

        public Products EditProduct(int id)
        {
            // string connection = ConfigurationManager.ConnectionStrings["E_Mart"].ConnectionString;
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            Products pro = new Products();
            SqlCommand cmd = new SqlCommand("Sp_EditProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("ProductId", id);
            SqlDataReader sdr = cmd.ExecuteReader();
            if(sdr.HasRows)
            {
                
                if (sdr.Read())
                {
                
                    pro.ProductId = int.Parse(sdr["ProductId"].ToString());
                    pro.ProductName = sdr["ProductName"].ToString();
                    pro.ProductPrice = int.Parse(sdr["ProductPrice"].ToString());
                    pro.ProductQuality = sdr["ProductQuality"].ToString();
                    pro.ProductImage = sdr["ProductImage"].ToString();

                   pro.CategoryName=sdr["CategoryName"].ToString();
                
                }

             
            }


            con.Close();
            return (pro);




        }

        public void UpdateProduct(Products Products)
        {
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_UpdateProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("ProductId", Products.ProductId);
            cmd.Parameters.AddWithValue("ProductName", Products.ProductName);
            cmd.Parameters.AddWithValue("ProductPrice", Products.ProductPrice);
            cmd.Parameters.AddWithValue("ProductQuality", Products.ProductQuality);
            cmd.Parameters.AddWithValue("ProductImage", Products.ProductImage);
            cmd.Parameters.AddWithValue("CategoryName", Products.CategoryName);
            cmd.ExecuteNonQuery();
        }

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

        public void AddToCart(string productId, string CustomerId, string CartStatus)
        {
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("AddToCart", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
           // cmd.Parameters.AddWithValue("CartStatus",CartStatus);
            cmd.ExecuteNonQuery();
        }

        public List<Products> CheckOut(object p)
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

                    product.Add(pro);
                }


            }

            return product;
        }
    }
}