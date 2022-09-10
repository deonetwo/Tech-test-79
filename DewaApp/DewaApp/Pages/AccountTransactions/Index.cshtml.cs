using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DewaApp.Pages.AccountTransactions
{
    public class IndexModel : PageModel
    {
        public List<AccountTransaction> AccountTransactions = new List<AccountTransaction>();

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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM AccountTransactions";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AccountTransaction accountTransaction = new AccountTransaction();
                                accountTransaction.TransactionDate = DateOnly.FromDateTime(reader.GetDateTime(1));
                                accountTransaction.Description = reader.GetString(2);
                                accountTransaction.DebitCreditStatus = reader.GetString(3);
                                accountTransaction.Amount = reader.GetDecimal(4).ToString("#,###");
                                accountTransaction.AccountId = reader.GetInt32(5);

                                AccountTransactions.Add(accountTransaction);
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
    }

    public class AccountTransaction
    {
        public DateOnly TransactionDate;
        public string Description;
        public string DebitCreditStatus;
        public string Amount;
        public int AccountId;
    }
}
