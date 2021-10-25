using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PointOfSale_Application.Classes
{
    public class ReceiptModel
    {
        public int report_id { get; set; }
        public int staff_id { get; set; }
        public string sales_type { get; set; }
        public string address { get; set; }
        public decimal paid_amount { get; set; }
        public DateTime date_issued { get; set; }

        public static List<ReceiptModel> GetRecords()
        {
            List<ReceiptModel> li = new List<ReceiptModel>();
            SqlConnection con = DBConnection.GetConnection();

            try
            {
                con.Open();
                String query = "SELECT * FROM [product_report]";
                SqlCommand sql = new SqlCommand(query, con);
                sql.CommandType = CommandType.Text;
                SqlDataReader dr = sql.ExecuteReader();

                while (dr.Read())
                {
                    ReceiptModel temp = new ReceiptModel();

                    temp.report_id = (int)dr["report_id"];
                    temp.staff_id = (int)dr["staff_id"];
                    temp.sales_type = dr["sales_type"].ToString();
                    temp.address = dr["address"].ToString();
                    temp.paid_amount = (decimal)dr["paid_amount"];
                    temp.date_issued = (DateTime)dr["date_issued"];

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

        public static int GetMaxID()
        {
            int maxid = 0;
            SqlConnection con = DBConnection.GetConnection();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            try
            {
                int id = 0;
                cmd.CommandText = "SELECT MAX(report_id) FROM [product_report]";
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
                throw new Exception("Error getting MAX ID : " + error.Message);
            }
            finally
            {
                cmd.Parameters.Clear();
                con.Close();
            }
        }

        public static void SaveReceipt(ReceiptModel receipt)
        {
            SqlConnection con = DBConnection.GetConnection();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            try
            {
                cmd.CommandText = "INSERT INTO [product_report] (report_id, staff_id, sales_type, address, paid_amount, date_issued) VALUES (@report_id, @staff_id, @type, @address, @amount, @date)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@report_id", receipt.report_id);
                cmd.Parameters.AddWithValue("@staff_id", receipt.staff_id);
                cmd.Parameters.AddWithValue("@type", receipt.sales_type);
                cmd.Parameters.AddWithValue("@address", receipt.address);
                cmd.Parameters.AddWithValue("@amount", receipt.paid_amount);
                cmd.Parameters.AddWithValue("@date", DateTime.Today);
                cmd.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                throw new Exception("Error in saving receipt : " + error.Message);

            }
            finally
            {
                cmd.Parameters.Clear();
                con.Close();
            }
        }
    }
}
