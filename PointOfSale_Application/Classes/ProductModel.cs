using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale_Application.Classes
{
    public class ProductModel
    {
        public int prod_id { get; set; }
        public string prod_name { get; set; }
        public string brand { get; set; }
        public int qty { get; set; }
        public decimal msrp { get; set; }
        public string status { get; set; }


        public static List<ProductModel> GetRecords()
        {
            List<ProductModel> li = new List<ProductModel>();
            SqlConnection con = DBConnection.GetConnection();

            try
            {
                con.Open();
                String query = "SELECT * FROM [product_tbl] pt INNER JOIN [product_status_tbl] pst ON pt.prod_id = pst.prod_id";
                SqlCommand sql = new SqlCommand(query, con);
                sql.CommandType = CommandType.Text;
                SqlDataReader dr = sql.ExecuteReader();

                while (dr.Read())
                {
                    ProductModel temp = new ProductModel();

                    temp.prod_id = (int)dr["prod_id"];
                    temp.prod_name = dr["name"].ToString();
                    temp.brand = dr["brand"].ToString();
                    temp.qty = (int)dr["qty"];
                    temp.msrp = (decimal)dr["msrp"];
                    temp.status = dr["status"].ToString();

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

        public static List<ProductModel> GetGoodProducts()
        {
            List<ProductModel> li = new List<ProductModel>();
            SqlConnection con = DBConnection.GetConnection();

            try
            {
                con.Open();
                String query = "SELECT * FROM [product_tbl] pt INNER JOIN [product_status_tbl] pst ON pt.prod_id = pst.prod_id WHERE status=@status";
                SqlCommand sql = new SqlCommand(query, con);
                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@status", "Good");
                SqlDataReader dr = sql.ExecuteReader();

                while (dr.Read())
                {
                    ProductModel temp = new ProductModel();

                    temp.prod_id = (int)dr["prod_id"];
                    temp.prod_name = dr["name"].ToString();
                    temp.brand = dr["brand"].ToString();
                    temp.qty = (int)dr["qty"];
                    temp.msrp = (decimal)dr["msrp"];
                    temp.status = dr["status"].ToString();

                    li.Add(temp);
                }

                return li;

            }
            catch (Exception error)
            {
                throw new Exception("Error in getting good products: " + error.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public static List<ProductModel> GetDefectiveProducts()
        {
            List<ProductModel> li = new List<ProductModel>();
            SqlConnection con = DBConnection.GetConnection();

            try
            {
                con.Open();
                String query = "SELECT * FROM [product_tbl] pt INNER JOIN [product_status_tbl] pst ON pt.prod_id = pst.prod_id WHERE status=@status AND qty > 0";
                SqlCommand sql = new SqlCommand(query, con);
                sql.Parameters.AddWithValue("@status", "Defective");
                sql.CommandType = CommandType.Text;
                SqlDataReader dr = sql.ExecuteReader();

                while (dr.Read())
                {
                    ProductModel temp = new ProductModel();

                    temp.prod_id = (int)dr["prod_id"];
                    temp.prod_name = dr["name"].ToString();
                    temp.brand = dr["brand"].ToString();
                    temp.qty = (int)dr["qty"];
                    temp.msrp = (decimal)dr["msrp"];
                    temp.status = dr["status"].ToString();

                    li.Add(temp);
                }

                return li;
            }
            catch (Exception error)
            {
                throw new Exception("Error in getting defective products: " + error.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public static int GetMaxID(string type)
        {
            int maxid = 0;
            SqlConnection con = DBConnection.GetConnection();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            try
            {
                int id = 0;
                if (type == "status")
                {
                    cmd.CommandText = "SELECT MAX(status_id) FROM [product_status_tbl]";
                }
                else if (type == "product")
                {
                    cmd.CommandText = "SELECT MAX(prod_id) FROM [product_tbl]";
                }

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string val = dr[0].ToString();
                    if (val == "")
                    {
                        maxid = 1;
                    }
                    else
                    {
                        id = Convert.ToInt32(dr[0].ToString());
                        id += 1;
                        maxid = id;
                    }
                }
                return maxid;
            }
            catch (Exception error)
            {
                throw new Exception("Error getting MAX ID" + error.Message);
            }
            finally
            {
                cmd.Parameters.Clear();
                con.Close();
            }
        }

        public static void Add_ProductModel(int prod_id, string name, string brand, decimal msrp)
        {
            SqlConnection con = DBConnection.GetConnection();
            string query = "INSERT INTO product_tbl (prod_id, name, brand, msrp) VALUES (@prod_id, @name, @brand, @msrp)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;

            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@prod_id", prod_id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("msrp", msrp);
                cmd.ExecuteReader();
            }
            catch (Exception error)
            {
                throw new Exception("Error in adding to product_tbl : " + error.Message);
            }
            finally
            {
                cmd.Parameters.Clear();
                con.Close();
            }
        }

        public static void Add_ProductQuantity(int status_id, int prod_id, int qty)
        {
            SqlConnection con = DBConnection.GetConnection();
            string query = "INSERT INTO product_status_tbl (status_id, prod_id, qty, status) VALUES (@status_id, @prod_id, @qty, 'Good')";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;

            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("status_id", status_id);
                cmd.Parameters.AddWithValue("@prod_id", prod_id);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                throw new Exception("Error in adding to product_status_tbl : " + error.Message);
            }
            finally
            {
                cmd.Parameters.Clear();
                con.Close();
            }
        }
    }
}
