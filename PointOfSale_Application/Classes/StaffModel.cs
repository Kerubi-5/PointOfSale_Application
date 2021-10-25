using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PointOfSale_Application.POS;

namespace PointOfSale_Application.Classes
{
    public class StaffModel
    {
        public int staff_id { get; set; }
        public string staff_name { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string username { get; set; }

        public static List<StaffModel> GetRecords()
        {
            List<StaffModel> li = new List<StaffModel>();
            SqlConnection con = DBConnection.GetConnection();

            try
            {
                con.Open();
                String query = "SELECT * FROM [staff_tbl]";
                SqlCommand sql = new SqlCommand(query, con);
                sql.CommandType = CommandType.Text;
                SqlDataReader dr = sql.ExecuteReader();

                while(dr.Read())
                {
                    StaffModel temp = new StaffModel();

                    temp.staff_id = (int)dr["staff_id"];
                    temp.staff_name = dr["staff_name"].ToString();
                    temp.phone = dr["phone"].ToString();
                    temp.password = dr["password"].ToString();
                    temp.username = dr["username"].ToString();

                    li.Add(temp);
                }

                return li;

            }
            catch
            {
                throw new Exception("Error in getting list of staff records from the database");
            }
            finally
            {
                con.Close();
            }
        }

        public static bool VerifyLogin(string username, string password)
        {
            StaffModel temp = new StaffModel();
            SqlConnection con = DBConnection.GetConnection();
            bool res = false;

            try
            {
                con.Open();
                String query = "SELECT COUNT(1) FROM [staff_tbl] WHERE username=@username AND password=@password";
                SqlCommand sql = new SqlCommand(query, con);
                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@username", username);
                sql.Parameters.AddWithValue("@password", password);
                int count = Convert.ToInt32(sql.ExecuteScalar());
                
                if (count == 1)
                {
                    res = true;
                }
                return res;
            }
            catch (Exception error)
            {
                throw new Exception("Error in verifying login : " + error.Message);
            }
            finally
            {
                con.Close();
            }

        }

        public static StaffModel GetStaffRecordByUserPass(string username, string password)
        {
            StaffModel temp = new StaffModel();
            SqlConnection con = DBConnection.GetConnection();

            try
            {
                con.Open();
                String query = "SELECT * FROM [staff_tbl] WHERE username=@username AND password=@password";
                SqlCommand sql = new SqlCommand(query, con);
                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@username", username);
                sql.Parameters.AddWithValue("@password", password);
                SqlDataReader dr = sql.ExecuteReader();

                if (dr.Read())
                {
                    temp.staff_id = (int)dr["staff_id"];
                    temp.staff_name = dr["staff_name"].ToString();
                    temp.phone = dr["phone"].ToString();
                    temp.password = dr["password"].ToString();
                    temp.username = dr["username"].ToString();
                }

                return temp;
            }
            catch
            {
                throw new Exception("Error in getting staff record");
            }
            finally
            {
                con.Close();
            }
        }
    }
}
