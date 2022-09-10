using DewaApp.Pages.AccountTransactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DewaApp.Pages.AccountTransactionsHistory
{
    public class IndexModel : PageModel
    {
        public List<AccountTransactionHistory> accountTransactionHistories = new List<AccountTransactionHistory>();
        public AccountHistoryDateRange accounthistoryDateRange = new AccountHistoryDateRange();
        public string errorMsg = "";

        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            try
            {
                string sql = "";
                bool sqlWithFilter = false;
                if (accounthistoryDateRange.AccountId != null)
                {
                    sql = "SELECT * FROM AccountTransactions WHERE AccountId=@id AND TransactionDate>=@startDate AND TransactionDate<=@endDate";
                    sqlWithFilter = true;
                }
                else
                {
                    sql = "SELECT * FROM AccountTransactions";
                    sqlWithFilter = false;
                }

                string connectionString = _configuration.GetConnectionString("default");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (sqlWithFilter)
                        {
                            command.Parameters.AddWithValue("id", accounthistoryDateRange.AccountId);
                            command.Parameters.AddWithValue("startDate", accounthistoryDateRange.StartDate);
                            command.Parameters.AddWithValue("endDate", accounthistoryDateRange.EndDate);
                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AccountTransactionHistory accountTransactionHistory = new AccountTransactionHistory();
                                accountTransactionHistory.TransactionDate = DateOnly.FromDateTime(reader.GetDateTime(1));
                                accountTransactionHistory.Description = reader.GetString(2);
                                accountTransactionHistory.Debit = reader.GetString(3)[0] == 'D' ? reader.GetDecimal(4).ToString("#,###") : "-";
                                accountTransactionHistory.Credit = reader.GetString(3)[0] == 'C' ? reader.GetDecimal(4).ToString("#,###") : "-";
                                accountTransactionHistory.Amount = reader.GetDecimal(4).ToString("#,###");

                                accountTransactionHistories.Add(accountTransactionHistory);
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

        public void OnPost()
        {
            try
            {
                accounthistoryDateRange.AccountId = Int32.Parse(Request.Form["accountId"]);
                accounthistoryDateRange.StartDate = Convert.ToDateTime(Request.Form["transactionDateStart"]);
                accounthistoryDateRange.EndDate = Convert.ToDateTime(Request.Form["transactionDateEnd"]);

                if (accounthistoryDateRange.StartDate > accounthistoryDateRange.EndDate)
                {
                    errorMsg = "Transaction start date must not be earlier than end date.";
                    accounthistoryDateRange.AccountId = null;
                    return;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }

            OnGet();
        }
    }

    public class AccountTransactionHistory
    {
        public DateOnly TransactionDate;
        public string Description;
        public string Credit;
        public string Debit;
        public string Amount;
    }

    public class AccountHistoryDateRange
    {
        public int? AccountId;
        public DateTime? StartDate;
        public DateTime? EndDate;
    }
}
