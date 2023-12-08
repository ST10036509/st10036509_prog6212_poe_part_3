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
        //declare variables
        string connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433; Initial Catalog = db-vc-prog6212-st10036509-part-2; Persist Security Info=False; User ID = ST10036509; Password=Randomsangh72; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout = 30;";
        
        // GET: /<controller>/
        [HttpGet]
        //create action method
        public IActionResult Login()
        {
            //create connection
            SqlConnection cnn = new SqlConnection(connectionString);
            //create query
            string query = "SELECT UserID, Username, Password FROM Users";
            //create list to store users
            List<UserModel> users = new List<UserModel>();

            //create command
            using (SqlCommand command = new SqlCommand(query, cnn))
            {
                //open connection
                cnn.Open();
                //create reader
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //read each record
                    while (reader.Read())
                    {
                        //create user object
                        UserModel user = new UserModel
                        {
                            UserID = (int)reader["UserID"],
                            Username = (string)reader["Username"],
                            Password = (string)reader["Password"],
                        };
                        //add user to list
                        users.Add(user);
                    }
                }
                //close connection
                cnn.Close();
            }
            //return view with list of users
            return View(users);
        }//end Login method

        // POST: /<controller>/
        [HttpPost]
        //create action method
        public async Task<IActionResult> Login(string username, string password, string userModelList)
        {
            //deserialize json string to list of users
            List<UserModel> users = JsonSerializer.Deserialize<List<UserModel>>(userModelList);
            //check if username exists
            bool usernameExists = users.Any(u => u.Username == username);
            //if username exists
            if (usernameExists)
            {
                //get user object
                UserModel user = users.FirstOrDefault(u => u.Username == username);
                //validate password
                PasswordHandler handler = new PasswordHandler();
                var validPassword = await Task.Run(() => handler.ValidatePassword(dbPassword: user.Password, password));

                //if password is invalid
                if (!validPassword)
                {
                    //return view with error message
                    TempData["ErrorMessage"] = "Invalid password!";
                    return View(users);
                }
                else
                {
                    //if password is valid
                    ViewBag.UserID = user.UserID;
                    //return view with user id
                    return RedirectToAction("About", "Home", new {user.UserID});
                }
            }
            else
            {
                //if username does not exist
                TempData["ErrorMessage"] = "Invalid username!";
                return View(users);
            }
        }//end Login method

        // GET: /<controller>/
        [HttpGet]
        //create action method
        public IActionResult Register()
        {
            //create connection
            SqlConnection cnn = new SqlConnection(connectionString);
            //create query
            string query = "SELECT Username, Password FROM Users";
            //create list to store users
            List<UserModel> users = new List<UserModel>();

            //create command
            using (SqlCommand command = new SqlCommand(query, cnn))
            {
                //open connection
                cnn.Open();

                //create reader
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //read each record
                    while (reader.Read())
                    {
                        //create user object
                        UserModel user = new UserModel
                        {
                            Username = (string)reader["Username"],
                            Password = (string)reader["Password"],
                        };
                        //add user to list
                        users.Add(user);
                    }
                }
                //close connection
                cnn.Close();
            }
            //return view with list of users
            return View(users);
        }//end Register method

        // POST: /<controller>/
        [HttpPost]
        //create action method
        public async Task<IActionResult> Register(string username, string password, string confirmPassword, string userModelList)
        {
            //deserialize json string to list of users
            List<UserModel> users = JsonSerializer.Deserialize<List<UserModel>>(userModelList);
            //check if username exists
            bool userExists = users.Any(u => u.Username == username);
            //if username exists
            if (userExists)
            {
                //return view with error message
                TempData["ErrorMessage"] = "Username Unavaliable!";
                //return view with list of users
                return View(users);
            }//if password is not confirmed
            else if (password != confirmPassword)
            {
                //return view with error message
                TempData["ErrorMessage"] = "Passwords Dont Match!";
                //return view with list of users
                return View(users);
            }
            else
            {
                //hash password
                PasswordHandler handler = new PasswordHandler();
                var hashedPassword = await Task.Run(() => handler.EncryptPassword(password));

                //add user to database
                var userID = await Task.Run(() => AddUser(username, hashedPassword));

                //return view with user id
                ViewBag.UserID = userID;
                //return view with user id
                return RedirectToAction("About", "Home", new {userID});
            }
        }//end Register method

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
//__________________________________________....oooOO0_END_OF_FILE_0OOooo....__________________________________________