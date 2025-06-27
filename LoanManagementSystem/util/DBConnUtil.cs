using System.Configuration;
using System.Data.SqlClient;

namespace LoanManagementSystem.util
{
    public class DBConnUtil
    {
        public static SqlConnection GetConnection()
        {
            string connString = ConfigurationManager.ConnectionStrings["LoanDB"].ConnectionString;
            return new SqlConnection(connString);
        }
    }
}