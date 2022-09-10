using DewaApp.Pages.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Principal;

namespace DewaApp.Pages.AccountTransactions
{
    public class InsertModel : PageModel
    {
        public AccountTransaction accountTransaction = new AccountTransaction();
        public string errorMsg = "";
        public string successMsg = "";

        private readonly IConfiguration _configuration;

        public InsertModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (Request.Form["accountId"] == "" || Request.Form["transactionDate"] == "" ||
                Request.Form["description"] == "" || Request.Form["debitCreditStatus"] == "" ||
                Request.Form["amount"] == 0)
            {
                errorMsg = "All field must not be empty";
                return;
            }

            accountTransaction.AccountId = Int32.Parse(Request.Form["accountId"]);
            accountTransaction.TransactionDate = DateOnly.FromDateTime(Convert.ToDateTime(Request.Form["transactionDate"]));
            accountTransaction.Description = Request.Form["description"];
            accountTransaction.DebitCreditStatus = Request.Form["debitCreditStatus"];
            accountTransaction.Amount = Request.Form["amount"];

            try
            {
                string connectionString = _configuration.GetConnectionString("default");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO AccountTransactions (TransactionDate, Description, DebitCreditStatus, Amount, AccountId)" +
                        "VALUES (@transactionDate, @desc, @dcStatus, @amount, @accountId);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("transactionDate", DateTime.Parse(accountTransaction.TransactionDate.ToString()));
                        command.Parameters.AddWithValue("desc", accountTransaction.Description);
                        command.Parameters.AddWithValue("dcStatus", accountTransaction.DebitCreditStatus[0]);
                        command.Parameters.AddWithValue("amount", Decimal.Parse(accountTransaction.Amount));
                        command.Parameters.AddWithValue("accountId", accountTransaction.AccountId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }

            accountTransaction.AccountId = 0;
            accountTransaction.TransactionDate = DateOnly.FromDateTime(DateTime.Now);
            accountTransaction.DebitCreditStatus = "";
            accountTransaction.Amount = "";
            successMsg = "New Transaction added successfully";

            Response.Redirect("/AccountTransactions/Index");
        }
    }
}
