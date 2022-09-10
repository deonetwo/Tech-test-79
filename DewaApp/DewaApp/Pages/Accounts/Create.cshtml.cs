using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DewaApp.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        public Account account = new Account();
        public string errorMsg = "";
        public string successMsg = "";

        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (Request.Form["accountId"] == "" || Request.Form["name"] == "")
            {
                errorMsg = "All field must not be empty";
                return;
            }

            account.AccountId = Int32.Parse(Request.Form["accountId"]);
            account.Name = Request.Form["name"];

            try
            {
                string connectionString = _configuration.GetConnectionString("default");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Accounts (AccountId, Name) VALUES (@id, @name);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", account.AccountId);
                        command.Parameters.AddWithValue("name", account.Name);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }

            account.AccountId = 0;
            account.Name = "";
            successMsg = "New Account added successfully";

            Response.Redirect("/Accounts/Index");
        }
    }
}
