using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PointOfSale_Application.Classes
{
    public class Product_Transaction_Model
    {
        public int prod_id { get; set; }
        public int report_id { get; set; }
        public string name { get; set; }
        public int qty_transacted { get; set; }
        public decimal total_amount { get; set; }

        public static List<Product_Transaction_Model> GetRecords()
        {
            List<Product_Transaction_Model> li = new List<Product_Transaction_Model>();
            SqlConnection con = DBConnection.GetConnection();

            try
            {
                con.Open();
                String query = "SELECT * FROM [product_in_transaction]";
                SqlCommand sql = new SqlCommand(query, con);
                sql.CommandType = CommandType.Text;
                SqlDataReader dr = sql.ExecuteReader();

                while (dr.Read())
                {
                    Product_Transaction_Model temp = new Product_Transaction_Model();

                    temp.prod_id = (int)dr["prod_id"];
                    temp.report_id = (int)dr["report_id"];
                    temp.name = dr["name"].ToString();
                    temp.qty_transacted = (int)dr["qty_transacted"];
                    temp.total_amount = (decimal)dr["total_amount"];

                    li.Add(temp);
                }

                return li;

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public static void Save_Production_Transaction(int report_id, Product_Transaction_Model product_transaction)
        {
            SqlConnection con = DBConnection.GetConnection();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO [product_in_transaction] (report_id, prod_id, name, qty_transacted, total_amount) VALUES (@report_id, @prod_id, @name, @qty_transacted, @total_amount)";

            try
            {
                cmd.Parameters.AddWithValue("@report_id", report_id);
                cmd.Parameters.AddWithValue("@prod_id", product_transaction.prod_id);
                cmd.Parameters.AddWithValue("@name", product_transaction.name);
                cmd.Parameters.AddWithValue("@qty_transacted", product_transaction.qty_transacted);
                cmd.Parameters.AddWithValue("@total_amount", product_transaction.total_amount);
                cmd.ExecuteNonQuery();

            }
            catch (Exception error)
            {
                throw new Exception("Error in saving transaction to product in transaction: " + error.Message);
            }
            finally
            {
                cmd.Parameters.Clear();
                con.Close();
            }
        }
    }
}
