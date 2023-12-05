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
        string connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433; Initial Catalog = db-vc-prog6212-st10036509-part-2; Persist Security Info=False; User ID = ST10036509; Password=Randomsangh72; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout = 30;";
        
        [HttpGet]
        public IActionResult Login()
        {
            SqlConnection cnn = new SqlConnection(connectionString);
            string query = "SELECT UserID, Username, Password FROM Users";

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
                            UserID = (int)reader["UserID"],
                            Username = (string)reader["Username"],
                            Password = (string)reader["Password"],
                        };
                        users.Add(user);
                    }
                }
                cnn.Close();
            }
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string userModelList)
        {
            List<UserModel> users = JsonSerializer.Deserialize<List<UserModel>>(userModelList);

            bool usernameExists = users.Any(u => u.Username == username);

            if (usernameExists)
            {
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
                    ViewBag.UserID = user.UserID;
                    return RedirectToAction("About", "Home", new {user.UserID});
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username!";
                return View(users);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
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

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword, string userModelList)
        {
            List<UserModel> users = JsonSerializer.Deserialize<List<UserModel>>(userModelList);

            bool userExists = users.Any(u => u.Username == username);

            if (userExists)
            {
                TempData["ErrorMessage"] = "Username Unavaliable!";
                return View(users);
            }
            else if (password != confirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords Dont Match!";
                return View(users);
            }
            else
            {
                PasswordHandler handler = new PasswordHandler();
                var hashedPassword = await Task.Run(() => handler.EncryptPassword(password));

                var userID = await Task.Run(() => AddUser(username, hashedPassword));

                ViewBag.UserID = userID;
                return RedirectToAction("About", "Home", new {username});
            }
        }

        public async Task<int> AddUser(string username, string password)
        {
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            //create insert query
            string query = "INSERT INTO Users (Username, [Password]) OUTPUT INSERTED.UserID VALUES (@Username, @Password);";
            //make a new command
            SqlCommand command = new SqlCommand(query, cnn);
            //insert variable values into query
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            //run query and get the inserted records UserID
            int userID = (int)await command.ExecuteScalarAsync();
            cnn.Close();
            return userID;
        }//end AddUser method
    }
}
