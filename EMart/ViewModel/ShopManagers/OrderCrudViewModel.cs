using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using EMart.Models;
using System.Configuration;
using EMart.Report;

namespace EMart.ViewModel.ShopManagers
{
    public class OrderCrudViewModel
    {

        string connection = ConfigurationManager.ConnectionStrings["E_MART"].ConnectionString;
        // Get All Orders
        public List<Order> GetAllOrders()
        {
            List<Order> OrderList = new List<Order>();
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Order_GetAllOrders",con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "All");
            cmd.Parameters.AddWithValue("@ShopId", "");
            SqlDataReader sdr = cmd.ExecuteReader();
            if(sdr.HasRows)
            {
                while(sdr.Read())
                {
                    Order order = new Order();
                    order.OrderId = int.Parse(sdr["OrderId"].ToString());
                    order.OrderAddress = sdr["OrderAddress"].ToString();
                    //order.OrderShopingId = int.Parse(sdr["OrderShopingId"].ToString());
                    order.GrandTotal = int.Parse(sdr["GrandTotal"].ToString());
                   // order.PaymentStatus = sdr["PaymentStatus"].ToString();
                    OrderList.Add(order);
                }
            }
            con.Close();
            return OrderList;
        }

        public List<OrderDetailView> GetOrderDetail(int OrderId=0)
        {
            List<OrderDetailView> orderview = new List<OrderDetailView>();

            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Order_GetOrderDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", OrderId);
            cmd.Parameters.AddWithValue("@Action", "All");
            cmd.Parameters.AddWithValue("@ShopId", "");
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while(sdr.Read())
                {
                    OrderDetailView detail = new OrderDetailView();
                    Order order = new Order();
                    order.OrderId = int.Parse(sdr["OrderId"].ToString());
                    order.OrderAddress = sdr["OrderAddress"].ToString();
                    order.OrderCustomerId =int.Parse(sdr["OrderCustomerId"].ToString());
                    //order.OrderShopingId = int.Parse(sdr["OrderShopingId"].ToString());
                    order.GrandTotal = int.Parse(sdr["GrandTotal"].ToString());
                    order.OrderPaymentType = sdr["OrderPaymentType"].ToString();
                    // order.PaymentStatus = sdr["PaymentStatus"].ToString();
                    detail.Order = order;
                    Products product = new Products();
                    product.ProductId = int.Parse(sdr["ProductId"].ToString());
                    product.ProductName = sdr["ProductName"].ToString();
                    product.ProductPrice = int.Parse(sdr["ProductPrice"].ToString());
                    product.ProductImage = sdr["ProductImage"].ToString();
                    product.ProductQuality = sdr["ProductQuality"].ToString();
                    //product.CategoryName = sdr["CategoryName"].ToString();
                    detail.Product = product;
                    orderview.Add(detail);
                }
            }
            con.Close();
            return orderview;
        }

        public DataSet ConfirmOrder(int id = 0, int ShopId = 0, int CustomerId = 0, int Total = 0, string PaymentType = "")
        {
            List<Order> OrderList = new List<Order>();
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Order_ConfirmOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", id);
            cmd.Parameters.AddWithValue("@ShopId", ShopId);
            cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
            cmd.Parameters.AddWithValue("@Total", Total);
            cmd.Parameters.AddWithValue("@PaymentType", PaymentType);
            SqlDataReader dr = cmd.ExecuteReader();
  //          SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet DataSet= new StoreBillDataSet();
            DataTable dt = new DataTable();
   //         sda.Fill(dt);
            if(dr.HasRows)
            {
                DataSet.Tables["tbl_Order"].Load(dr);
//                sda.Fill(DataSet, "tbl_Order");
                SqlCommand cmd2 = new SqlCommand("sp_Order_BillGenerateData", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@ShopId", ShopId);
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                sda1.Fill(dt2);
                if(dt2.Rows.Count>0)
                {
                    sda1.Fill(DataSet, "tbl_User");
                    sda1.Fill(DataSet, "tbl_ShopKeeper");
                }
                SqlCommand cmd3 = new SqlCommand("sp_Order_ConfirmOrderGetCustomer", con);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@CustomerId", CustomerId);
                SqlDataAdapter sda2 = new SqlDataAdapter(cmd3);
                DataTable dt3 = new DataTable();
                sda1.Fill(dt3);
                if (dt3.Rows.Count > 0)
                {
                    sda2.Fill(DataSet, "tbl_Customer");
                }
            }
            con.Close();
            return DataSet;
        }

        // Get Live Orders
        public List<Order> GetLiveOrder(int id=0)
        {
            List<Order> OrderList = new List<Order>();
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Order_GetAllOrders", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "Live");
            cmd.Parameters.AddWithValue("@ShopId", id);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    Order order = new Order();
                    order.OrderId = int.Parse(sdr["OrderId"].ToString());
                    order.OrderAddress = sdr["OrderAddress"].ToString();
                    order.OrderShopingId = int.Parse(sdr["OrderShopingId"].ToString());
                    order.GrandTotal = int.Parse(sdr["GrandTotal"].ToString());
                    order.PaymentStatus = sdr["OrderPaymentStatus"].ToString();
                    OrderList.Add(order);
                }
            }
            con.Close();
            return OrderList;
        }

        // Get Live Order Detail

        public List<OrderDetailView> GetLiveOrderDetail(int OrderId = 0,int id=0)
        {
            List<OrderDetailView> orderview = new List<OrderDetailView>();

            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Order_GetOrderDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", OrderId);
            cmd.Parameters.AddWithValue("@Action", "Live");
            cmd.Parameters.AddWithValue("@ShopId", id);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    OrderDetailView detail = new OrderDetailView();
                    Order order = new Order();
                    order.OrderId = int.Parse(sdr["OrderId"].ToString());
                    order.OrderAddress = sdr["OrderAddress"].ToString();
                    order.OrderCustomerId = int.Parse(sdr["OrderCustomerId"].ToString());
                    order.OrderShopingId = int.Parse(sdr["OrderShopingId"].ToString());
                    order.GrandTotal = int.Parse(sdr["GrandTotal"].ToString());
                    order.PaymentStatus = sdr["OrderPaymentStatus"].ToString();
                    detail.Order = order;
                    Products product = new Products();
                    product.ProductId = int.Parse(sdr["ProductId"].ToString());
                    product.ProductName = sdr["ProductName"].ToString();
                    product.ProductPrice = int.Parse(sdr["ProductPrice"].ToString());
                    product.ProductImage = sdr["ProductImage"].ToString();
                    product.ProductQuality = sdr["ProductQuality"].ToString();
                    //product.CategoryName = sdr["CategoryName"].ToString();
                    detail.Product = product;
                    orderview.Add(detail);
                }
            }
            con.Close();
            return orderview;
        }

        //Get Order History

        // Get Live Orders
        public List<Order> GetOrderHistory(int id = 0,string Action="")
        {
            List<Order> OrderList = new List<Order>();
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Order_GetAllOrders", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", Action);
            cmd.Parameters.AddWithValue("@ShopId", id);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    Order order = new Order();
                    order.OrderId = int.Parse(sdr["OrderId"].ToString());
                    order.OrderAddress = sdr["OrderAddress"].ToString();
                    order.OrderShopingId = int.Parse(sdr["OrderShopingId"].ToString());
                    order.GrandTotal = int.Parse(sdr["GrandTotal"].ToString());
                    order.PaymentStatus = sdr["OrderPaymentStatus"].ToString();
                    OrderList.Add(order);
                }
            }
            con.Close();
            return OrderList;
        }

        public List<OrderDetailView> OrderHistoryDetail(int OrderId = 0, int id = 0,string Action="")
        {
            List<OrderDetailView> orderview = new List<OrderDetailView>();

            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Order_GetOrderDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", OrderId);
            cmd.Parameters.AddWithValue("@Action", Action);
            cmd.Parameters.AddWithValue("@ShopId", id);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    OrderDetailView detail = new OrderDetailView();
                    Order order = new Order();
                    order.OrderId = int.Parse(sdr["OrderId"].ToString());
                    order.OrderAddress = sdr["OrderAddress"].ToString();
                    order.OrderCustomerId = int.Parse(sdr["OrderCustomerId"].ToString());
                    order.OrderShopingId = int.Parse(sdr["OrderShopingId"].ToString());
                    order.GrandTotal = int.Parse(sdr["GrandTotal"].ToString());
                    order.PaymentStatus = sdr["OrderPaymentStatus"].ToString();
                    order.OrderPaymentType = sdr["OrderPaymentType"].ToString();
                    order.OrderStatus = sdr["OrderStatus"].ToString();
                    detail.Order = order;
                    Products product = new Products();
                    product.ProductId = int.Parse(sdr["ProductId"].ToString());
                    product.ProductName = sdr["ProductName"].ToString();
                    product.ProductPrice = int.Parse(sdr["ProductPrice"].ToString());
                    product.ProductImage = sdr["ProductImage"].ToString();
                    product.ProductQuality = sdr["ProductQuality"].ToString();
                    //product.CategoryName = sdr["CategoryName"].ToString();
                    detail.Product = product;
                    orderview.Add(detail);
                }
            }
            con.Close();
            return orderview;
        }







        //Order Completion

        public string OrderCompletion(int id)
        {
            string message = "";
            List<Order> OrderList = new List<Order>();
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Order_OrderCompletion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", id);
            if(cmd.ExecuteNonQuery()>0)
            {
                message = "Order Completed Successfully";
            }
            con.Close();
            return message;
        }


        /// Payments
        /// 
        public List<Payment> GetPaymentsDetail(int id = 0, string Action="")
        {
            List<Payment> PaymentList = new List<Payment>();
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Payment_GetAllPayments", con);
            cmd.CommandType = CommandType.StoredProcedure;
            // For Both Customer and Shop Keeper in One Sp
            cmd.Parameters.AddWithValue("@DataId", id);
            cmd.Parameters.AddWithValue("@Action", Action);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    Payment payment = new Payment();
                    payment.PaymentId = int.Parse(sdr["PaymentId"].ToString());
                    payment.PaymentOrderId = int.Parse(sdr["PaymentOrderId"].ToString());
                    payment.PaymentTotal = int.Parse(sdr["PaymentTotal"].ToString());
                    payment.PaymentMethod = sdr["PaymentMethod"].ToString();
                    payment.CustomerId = int.Parse(sdr["CustomerId"].ToString());
                    payment.ShopId = int.Parse(sdr["ShopId"].ToString());
                    payment.PaymentStatus = sdr["PaymentStatus"].ToString();
                    PaymentList.Add(payment);
                }
            }
            con.Close();
            return PaymentList;
        }

    }
}