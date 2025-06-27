using System.Data.Entity;

namespace LoanManagementSystem.util
{
    public class DBUtil
    {
        public static LoanDbContext GetDBConn()
        {
            return new LoanDbContext();
        }
    }
}
