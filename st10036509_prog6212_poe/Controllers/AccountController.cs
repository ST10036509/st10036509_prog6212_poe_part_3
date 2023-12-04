using HoursForYourLib;
using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace st10036509_prog6212_poe.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            string connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433; Initial Catalog = db-vc-prog6212-st10036509-part-2; Persist Security Info=False; User ID = ST10036509; Password=Randomsangh72; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout = 30;";
            SqlConnection cnn = new SqlConnection(connectionString);
            string query = "SELECT Username, Password FROM Users";

            List<UserModel> users = new List<UserModel>();

            using (SqlCommand command = new SqlCommand(query, cnn))
            {
                cnn.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel
                        {
                            Username = (string)reader["Username"],
                            Password = (string)reader["Password"],
                        };

                        users.Add(user);
                    }
                }

                cnn.Close();
            }
            //pull data and make a list of users models
            //send to view and handle the 
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string userModelList)
        {
            List<UserModel> users = JsonSerializer.Deserialize<List<UserModel>>(userModelList);

            bool usernameExists = users.Any(u => u.Username == username);

            if (usernameExists)
            {

                //HttpContext.Session.SetString("Username", username);

                UserModel user = users.FirstOrDefault(u => u.Username == username);

                PasswordHandler handler = new PasswordHandler();
                var validPassword = await Task.Run(() => handler.ValidatePassword(dbPassword: user.Password, password));

                if (!validPassword)
                {
                    TempData["ErrorMessage"] = "Invalid password!";
                    return View(users);
                }
                else
                {
                    return RedirectToAction("About", "Home");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username!";
                return View(users);
            }
        }


        public IActionResult Register()
        {
            return View();
        }
    }
}
