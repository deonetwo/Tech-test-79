using DewaApp.Pages.Accounts;
using DewaApp.Pages.AccountTransactions;
using DewaApp.Pages.AccountTransactionsHistory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DewaApp.Pages.AccountTransactionsPoint
{
    public class IndexModel : PageModel
    {
        public List<AccountTransactionPoint> accountTransactionPoints = new List<AccountTransactionPoint>();

        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("default");
                using (SqlConnection connection = new SqlConnection(connectionString + ";MultipleActiveResultSets=true"))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Accounts";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AccountTransactionPoint accountTransactionPoint = new AccountTransactionPoint();
                                accountTransactionPoint.AccountId = reader.GetInt32(1);
                                accountTransactionPoint.Name = reader.GetString(0);

                                int totalPulsaAmounts = 0;
                                int totalListrikAmounts = 0;
                                string getPointsSql = "SELECT SUM(Amount) FROM AccountTransactions WHERE AccountId=@id AND LOWER(Description)='beli pulsa';";
                                using (SqlCommand sqlCommand = new SqlCommand(getPointsSql, connection))
                                {
                                    sqlCommand.Parameters.AddWithValue("id", accountTransactionPoint.AccountId);
                                    totalPulsaAmounts = sqlCommand.ExecuteScalar() != DBNull.Value ? Decimal.ToInt32((decimal) sqlCommand.ExecuteScalar()) : 0;
                                }
                                getPointsSql = "SELECT SUM(Amount) FROM AccountTransactions WHERE AccountId=@id AND LOWER(Description)='bayar listrik';";
                                using (SqlCommand sqlCommand = new SqlCommand(getPointsSql, connection))
                                {
                                    sqlCommand.Parameters.AddWithValue("id", accountTransactionPoint.AccountId);
                                    totalListrikAmounts = sqlCommand.ExecuteScalar() != DBNull.Value ? Decimal.ToInt32((decimal)sqlCommand.ExecuteScalar()) : 0;
                                }

                                accountTransactionPoint.TotalPoints = getPulsaPoints(totalPulsaAmounts) + getListrikPoints(totalListrikAmounts);

                                accountTransactionPoints.Add(accountTransactionPoint);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private double getPulsaPoints(int amount)
        {
            double totalPoints = 0;
            double totalAmount = amount;
            if (10000 < totalAmount)
            {
                totalPoints += totalAmount < 30000 ?
                    ((totalAmount-10000) / 1000) * 1 :
                    (20000 / 1000) * 1;
            }
            if (30000 < totalAmount)
            {
                totalPoints += ((totalAmount-30000) / 1000) * 2;
            }
            return totalPoints;
        }

        private double getListrikPoints(int amount)
        {
            double totalPoints = 0;
            double totalAmount = amount;
            if (50000 < totalAmount)
            {
                totalPoints += totalAmount < 100000 ?
                    ((totalAmount - 50000) / 2000) * 1 :
                    (50000 / 2000) * 1;
            }
            if (100000 < totalAmount)
            {
                totalPoints += ((totalAmount-100000) / 2000) * 2;
            }
            return totalPoints;
        }
    }

    public class AccountTransactionPoint
    {
        public int AccountId;
        public string Name;
        public double TotalPoints;

    }
}
