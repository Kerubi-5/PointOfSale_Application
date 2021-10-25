using System.Data.SqlClient;

namespace PointOfSale_Application.Classes
{
    public class DBConnection
    {
        public static SqlConnection GetConnection()
        {
            //SqlConnection con = new SqlConnection();
            SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\C# Apps\\PointOfSale_Application\\PointOfSale_Application\\POS_DB.mdf\";Integrated Security=True");

            return con;
        }
    }
}
